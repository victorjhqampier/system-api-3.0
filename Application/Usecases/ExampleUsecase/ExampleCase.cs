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

    async public Task<EasyResult<ExecuteExampleTwoAdapter>> ExecuteExampleTwoAsync(TraceIdentifierAdapter header)
    {
        var arrValided = FluentValidationExecutor.Execute(header, new HeaderRequestAdapterValidator());
        if (arrValided.Any())
        {            
            return EasyResult<ExecuteExampleTwoAdapter>.Failure(403, arrValided);
        }

        var result = new ExecuteExampleTwoAdapter
        {
            Ping = "pong",
            Pong = "ping"
        };

        return EasyResult<ExecuteExampleTwoAdapter>.Success(result);
    }
}
