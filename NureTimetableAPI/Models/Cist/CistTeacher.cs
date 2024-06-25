using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistTeacher
{
    [JsonProperty("id")]
    public int TeacherId { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";
}
