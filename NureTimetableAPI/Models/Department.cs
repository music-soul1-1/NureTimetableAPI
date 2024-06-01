using Newtonsoft.Json;

namespace NureTimetableAPI.Models;

public class Department
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("teachers")]
    public List<Teacher> Teachers { get; set; } = [];
}