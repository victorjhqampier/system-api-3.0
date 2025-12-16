using System.Text.Json.Serialization;

namespace SystemAPI.Models.Internals;

public class FieldErrorInternalModel
{
    public string StatusCode { set; get; }
    public string Message { set; get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Field { set; get; }
}
