using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;

namespace NureTimetableAPI.Models;

public class AuditoryType
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";
}