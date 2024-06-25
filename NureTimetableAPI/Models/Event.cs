using Newtonsoft.Json;

namespace NureTimetableAPI.Models;

public class Event
{
    [JsonProperty("subject_id")]
    public int SubjectId { get; set; }

    [JsonProperty("start_time")]
    public int StartTime { get; set; }

    [JsonProperty("end_time")]
    public int EndTime { get; set; }

    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("number_pair")]
    public int NumberPair { get; set; }

    [JsonProperty("auditory")]
    public string Auditory { get; set; } = "";

    [JsonProperty("teachers")]
    public List<int> TeacherIds { get; set; } = [];

    [JsonProperty("groups")]
    public List<int> GroupIds { get; set; } = [];
}