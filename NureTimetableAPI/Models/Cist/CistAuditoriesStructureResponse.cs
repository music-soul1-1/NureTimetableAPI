using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistAuditoriesStructureResponse
{
    [JsonProperty("university")]
    public CistAuditoriesUniversity University { get; set; } = new();
}