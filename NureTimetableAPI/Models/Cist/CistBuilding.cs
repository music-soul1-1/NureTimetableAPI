using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistBuilding
{
    [JsonProperty("id")]
    public string Id { get; set; } = "";

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("auditories")]
    public List<CistAuditory> Auditories { get; set; } = [];
}