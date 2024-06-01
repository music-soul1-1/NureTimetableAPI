using Newtonsoft.Json;


namespace NureTimetableAPI.Models;

public class SubjectHour
{
    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("val")]
    public int Value { get; set; }

    [JsonProperty("teachers")]
    public List<int> TeacherIds { get; set; } = new List<int>();
}