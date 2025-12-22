using Domain.Entities;
using Domain.Interfaces;
using FakeApiInfrastructure.Collections;
using InternalHttpClientBuilder;
using Microsoft.Extensions.Logging;

namespace FakeApiInfrastructure.Queries;

public class FakeApiQueryInfra : IExampleTitleQuery
{
    private readonly HttpClientBuilder _httpClient;
    private readonly ILogger<FakeApiQueryInfra> _logger;

    public FakeApiQueryInfra(IHttpClientFactory factory, ILogger<FakeApiQueryInfra> logger)
    {
        _logger = logger;
        _httpClient = new HttpClientBuilder(factory, logger);
    }

    async public Task<ExampleTitleEntity> GetAsync(int value = 1)
    {
        var response = await _httpClient.Http("https://jsonplaceholder.typicode.com")
            .Endpoint($"todos/{value}")
            .GetAsync<ApiExampleCollection>();

        if (response.StatusCode != 200 || response.Content is null) return new ExampleTitleEntity();

        return new ExampleTitleEntity
        {
            Identity = response.Content!.Id,
            Title = response.Content!.Title
        };
    }

    async public Task<ExampleTitleEntity> GetProductAsync(int value = 1)
    {
        var response = await _httpClient.Http("https://fakestoreapi.com")
            .Endpoint($"products/{value}")
            .GetAsync<ApiExampleTwoCollection>();

        if (response.StatusCode != 200 || response.Content is null) return new ExampleTitleEntity();

        return new ExampleTitleEntity
        {
            Identity = response.Content!.Id,
            Title = response.Content!.Description
        };
    }
}
