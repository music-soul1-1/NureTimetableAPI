using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;
using NureTimetableAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models;

public class LessonType
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [JsonProperty("id")]
    public int TypeId { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    [JsonProperty("full_name")]
    public string FullName { get; set; } = "";

    [JsonProperty("id_base")]
    public int IdBase { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; } = "";

    public List<LessonDomain> Lessons { get; set; } = [];

    public LessonTypeDto ToDto()
    {
        return new LessonTypeDto
        {
            Id = TypeId,
            ShortName = ShortName,
            FullName = FullName,
            Type = Type,
            IdBase = IdBase,
        };
    }
}