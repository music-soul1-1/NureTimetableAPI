namespace NureTimetableAPI.Models.Dto;

public class AuditoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int? Floor { get; set; }

    public bool HasPower { get; set; }

    public List<AuditoryTypeDto> AuditoryTypes { get; set; } = [];

    public MinimalBuilding Building { get; set; } = new MinimalBuilding();
}
