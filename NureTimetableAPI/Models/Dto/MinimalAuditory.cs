namespace NureTimetableAPI.Models.Dto;

public class MinimalAuditory
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int? Floor { get; set; }

    public bool HasPower { get; set; }

    public List<AuditoryTypeDto> AuditoryTypes { get; set; } = [];
}
