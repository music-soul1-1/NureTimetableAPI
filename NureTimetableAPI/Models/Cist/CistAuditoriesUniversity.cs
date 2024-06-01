using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistAuditoriesUniversity
{
    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("buildings")]
    public List<CistBuilding> Buildings { get; set; } = [];
}
