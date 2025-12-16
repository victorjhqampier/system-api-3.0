using System.Text.Json.Serialization;

namespace SystemAPI.Models.Internals;

public class ResponseInternalModel
{    
    [JsonIgnore]
    public int StatusCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<FieldErrorInternalModel>? Errors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public dynamic? Response { set; get; }
}
