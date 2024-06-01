using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistTeachersUniversity
{
    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("faculties")]
    public List<CistTeachersFaculty> Faculties { get; set; } = [];
}