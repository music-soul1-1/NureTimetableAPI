using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistTeachersStructureResponse
{
    [JsonProperty("university")]
    public CistTeachersUniversity University { get; set; } = new();
}