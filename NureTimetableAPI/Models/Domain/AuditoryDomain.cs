using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models.Domain;

public class AuditoryDomain
{
    [Key]
    public Guid Id { get; set; }

    public int AuditoryId { get; set; }

    public string ShortName { get; set; } = "";

    public int? Floor { get; set; }

    public bool HasPower { get; set; }

    public List<AuditoryTypeDomain> AuditoryTypes { get; set; } = [];

    public Guid BuildingId { get; set; }

    public BuildingDomain Building { get; set; } = new BuildingDomain();
}
