using Newtonsoft.Json;


namespace NureTimetableAPI.Models;

public class Subject
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("brief")]
    public string Brief { get; set; } = "";

    [JsonProperty("title")]
    public string Title { get; set; } = "";

    [JsonProperty("hours")]
    public List<SubjectHour> Hours { get; set; } = new List<SubjectHour>();
}