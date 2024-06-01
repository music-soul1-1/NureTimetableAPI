using Newtonsoft.Json;

namespace NureTimetableAPI.Models;

public class LessonType
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("id_base")]
    public int IdBase { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; } = "";
}