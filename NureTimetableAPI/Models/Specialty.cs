using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;


namespace NureTimetableAPI.Models;

public class Specialty
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonProperty("id")]
    public int SpecialtyId { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("groups")]
    public List<Group> Groups { get; set; } = [];

    public Guid DirectionId { get; set; }

    public DirectionDomain Direction { get; set; } = new();
}