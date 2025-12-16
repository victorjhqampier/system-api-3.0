
using Application.Adapters;
using Application.Internals.Adapters;
using Application.Internals.Executors;

namespace Application.Ports;
public interface IExamplePort
{
    public Task<EasyResult<RetrieveExampleAdapter>> ShowExampleAsync(TraceIdentifierAdapter header);
}
