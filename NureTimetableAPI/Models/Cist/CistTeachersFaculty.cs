using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistTeachersFaculty
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("departments")]
    public List<CistTeachersInnerFaculty> InnerFaculties { get; set; } = [];
}
