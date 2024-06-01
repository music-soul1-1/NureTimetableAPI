using Newtonsoft.Json;

namespace NureTimetableAPI.Models.Cist;

public class CistAuditory
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = "";

    /// <summary>
    /// Can be null in Cist.
    /// </summary>
    [JsonProperty("floor")]
    public int? Floor { get; set; }

    [JsonProperty("is_have_power")]
    [JsonConverter(typeof(BoolConverter))]
    public bool HasPower { get; set; }

    [JsonProperty("auditory_types")]
    public List<AuditoryType> AuditoryTypes { get; set; } = [];
}

public class BoolConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue((bool)value ? "1" : "0");
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return reader.Value switch
        {
            "1" => true,
            "0" => false,
            _ => throw new JsonSerializationException("Invalid value for BoolConverter"),
        };
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool);
    }
}