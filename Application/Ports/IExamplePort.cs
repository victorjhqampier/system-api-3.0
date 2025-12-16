
using Application.Adapters;
using Application.Internals.Adapters;

namespace Application.Ports;
public interface IExamplePort
{
    public Task<(RetrieveExampleAdapter?, ValidationResponseAdapter?)> ShowExampleAsync(TraceIdentifierAdapter header);
}
