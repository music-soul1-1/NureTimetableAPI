using Newtonsoft.Json;


namespace NureTimetableAPI.Models.Cist;

public class CistSchedule
{
    [JsonProperty("time-zone")]
    public string TimeZone { get; set; } = "";

    [JsonProperty("events")]
    public List<Event> Events { get; set; } = [];

    [JsonProperty("groups")]
    public List<Group> Groups { get; set; } = [];

    [JsonProperty("subjects")]
    public List<Subject> Subjects { get; set; } = [];

    [JsonProperty("teachers")]
    public List<Teacher> Teachers { get; set; } = [];

    [JsonProperty("types")]
    public List<LessonType> Types { get; set; } = [];
}
