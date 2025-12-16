using Application.Adapters;
using Application.Internals.Adapters;
using Application.Internals.Executors;
using Application.Ports;

namespace Application.Usecases.ExampleUsecase;

public class ExampleCase : IExamplePort
{
    async public Task<EasyResult<RetrieveExampleAdapter>> ShowExampleAsync(TraceIdentifierAdapter header)
    {
        var arrValided = FluentValidationExecutor.Execute(header, new HeaderRequestAdapterValidator());
        if (arrValided.Any())
        {            
            return EasyResult<RetrieveExampleAdapter>.Failure(422, arrValided);
        }

        var result = new RetrieveExampleAdapter
        {
            Ping = "pong",
            Pong = "ping"
        };

        return EasyResult<RetrieveExampleAdapter>.Success(result);
    }
}
