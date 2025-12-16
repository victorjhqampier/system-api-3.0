namespace Application.Internals.Adapters;

public sealed class ValidationResponseAdapter
{
    public int StatusGroup { set; get; }
    public IReadOnlyCollection<ValidationError> Validations { get; init; } = Array.Empty<ValidationError>();
}

public sealed class ValidationError
{
    public string Code { set; get; }
    public string Message { set; get; }
    public string? Field { set; get; }
}
