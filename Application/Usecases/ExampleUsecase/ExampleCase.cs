using Application.Adapters;
using Application.Executors;
using Application.Internals.Adapters;
using Application.Ports;

namespace Application.Usecases.ExampleUsecase;

public class ExampleCase : IExamplePort
{
    async public Task<(RetrieveExampleAdapter?, ValidationResponseAdapter?)> ShowExampleAsync(TraceIdentifierAdapter header)
    {
        var validResult = FluentValidExecutor.Execute(header, new HeaderRequestAdapterValidator());
        if (validResult.Validations.Any())
        {
            return (null, validResult);
        }

        return (new RetrieveExampleAdapter
        {
            Ping = "pong",
            Pong = "ping"
        }, null);
    }
}
