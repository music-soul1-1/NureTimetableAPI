using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;


namespace NureTimetableAPI.Models;

public class Teacher
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonProperty("id")]
    public int TeacherId { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    public Guid DepartmentId { get; set; }

    public DepartmentDomain Department { get; set; } = new DepartmentDomain();
    
    public ScheduleFetchLog? ScheduleFetchLog { get; set; }

    public List<LessonDomain> Lessons { get; set; } = [];
}