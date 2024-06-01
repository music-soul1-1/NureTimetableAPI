using Newtonsoft.Json;


namespace NureTimetableAPI.Models.Cist;

public class CistGroupsUniversity
{
    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("faculties")]
    public List<CistGroupsFaculty> Faculties { get; set; } = [];
}
