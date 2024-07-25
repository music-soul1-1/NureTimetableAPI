using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistGroup
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";
}
