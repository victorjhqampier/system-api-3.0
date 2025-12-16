namespace Application.Internals.Adapters;

public sealed class ValidationResultAdapter
{
    public string Code { set; get; }
    public string Message { set; get; }
    public string? Field { set; get; }
    //public IReadOnlyCollection<ValidationError> Validations { get; init; } = Array.Empty<ValidationError>();
}