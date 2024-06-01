namespace NureTimetableAPI.Models.Domain;

public class BuildingDomain
{
    public Guid Id { get; set; }

    public string BuildingId { get; set; } = "";

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<AuditoryDomain> Auditories { get; set; } = [];
}
