using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;


namespace NureTimetableAPI.Models;

public class Group
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonProperty("id")]
    public int GroupId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    public Guid DirectionId { get; set; }

    public DirectionDomain Direction { get; set; } = new DirectionDomain();

    public ScheduleFetchLog? ScheduleFetchLog { get; set; }

    public List<LessonDomain> Lessons { get; set; } = [];
}