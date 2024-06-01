using Newtonsoft.Json;


namespace NureTimetableAPI.Models.Cist;

/// <summary>
///  Faculty from Cist (P_API_GROUP_JSON).
/// </summary>
public class CistGroupsFaculty
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("directions")]
    public List<Direction> Directions { get; set; } = [];
}