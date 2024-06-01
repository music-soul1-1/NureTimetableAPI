using Newtonsoft.Json;


namespace NureTimetableAPI.Models;

public class Direction
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("specialities")]
    public List<Specialty> Specialties { get; set; } = [];

    [JsonProperty("groups")]
    public List<Group> Groups { get; set; } = [];
}