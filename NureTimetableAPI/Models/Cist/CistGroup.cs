using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models.Cist;

public class CistGroup
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";
}
