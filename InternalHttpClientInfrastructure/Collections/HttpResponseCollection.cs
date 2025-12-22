using System.Net;
using System.Text.Json;

namespace InternalHttpClientInfrastructure.Collections;

public sealed record HttpResponseCollection<T>(
    int StatusCode,
    T? Content,
    Dictionary<string, string> Headers,
    string Url
);