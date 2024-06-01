using Newtonsoft.Json;


namespace NureTimetableAPI.Models.Cist;

public class CistGroupStructureResponse
{
    [JsonProperty("university")]
    public CistGroupsUniversity University { get; set; } = new();
}
