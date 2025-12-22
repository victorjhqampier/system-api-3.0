using InternalHttpClientInfrastructure.Collections;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

/* ********************************************************************************************************          
# * Copyright � 2025 Arify Labs - All rights reserved.   
# * 
# * Info                  : Http Client Builder.
# *
# * By                    : Victor Jhampier Caxi Maquera
# * Email/Mobile/Phone    : victorjhampier@gmail.com | 968991714
# *
# * Creation date         : 01/01/2026
# * 
# **********************************************************************************************************/

namespace InternalHttpClientBuilder;

public sealed class HttpClientBuilder
{
    private readonly IHttpClientFactory _factory;
    private static ILogger? _logger;

    private string _clientName = "ArifyClient";
    private string _baseUrl = "";
    private string _endpoint = "";
    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, string> _params = new();
    private readonly Dictionary<string, string> _query = new();

    public HttpClientBuilder(IHttpClientFactory factory, ILogger logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public HttpClientBuilder Client(string name)
    {
        _clientName = name;
        return this;
    }

    public HttpClientBuilder Http(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        return this;
    }

    public HttpClientBuilder Endpoint(string endpoint)
    {
        _endpoint = endpoint.TrimStart('/');
        return this;
    }

    public HttpClientBuilder Header(string key, string value)
    {
        _headers[key] = value;
        return this;
    }

    public HttpClientBuilder Authorization(string scheme, string token)
    {
        _headers["Authorization"] = $"{scheme} {token}";
        return this;
    }

    public HttpClientBuilder Param(string key, string value)
    {
        _params[key] = value;
        return this;
    }

    public HttpClientBuilder Query(string key, string value)
    {
        _query[key] = value;
        return this;
    }

    private Uri BuildUri()
    {
        var path = $"{_baseUrl}/{_endpoint}";
        var ub = new UriBuilder(path);

        if (_query.Count > 0)
        {
            var qs = string.Join("&", _query.Select(kv =>
                $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
            ub.Query = qs;
        }

        return ub.Uri;
    }

    private HttpRequestMessage BuildRequest(HttpMethod method, object? body = null)
    {
        var uri = BuildUri();        

        var req = new HttpRequestMessage(method, uri);

        foreach (var h in _headers)
            req.Headers.TryAddWithoutValidation(h.Key, h.Value);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }
        else if (!_headers.ContainsKey("Content-Type"))
        {
            // si necesitas forzar content-type solo cuando hay body, hazlo aquí
        }

        return req;
    }

    public async Task<HttpResponseCollection<T>> GetAsync<T>(CancellationToken ct = default)
    {
        var client = _factory.CreateClient(_clientName);
        using var req = BuildRequest(HttpMethod.Get);

        using var resp = await client.SendAsync(req, ct);
        return await BuildResponse<T>(resp);
    }

    public async Task<HttpResponseCollection<T>> PostAsync<T>(object? body, CancellationToken ct = default)
    {
        var client = _factory.CreateClient(_clientName);
        using var req = BuildRequest(HttpMethod.Post, body);

        using var resp = await client.SendAsync(req, ct);
        return await BuildResponse<T>(resp);
    }

    private static async Task<HttpResponseCollection<T>> BuildResponse<T>(HttpResponseMessage resp)
    {
        T? content = default;
        try
        {
            content = await resp.Content.ReadFromJsonAsync<T>();
        }
        catch 
        {
            _logger.LogError($"Swagger contract violation, Cannot decode Body in {(int)resp.StatusCode} {resp.RequestMessage?.RequestUri?.ToString()}");
        }

        return new HttpResponseCollection<T>(
            (int)resp.StatusCode,
            content, // is not null,
            resp.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value)),
            resp.RequestMessage?.RequestUri?.ToString() ?? ""
        );
    }
}
