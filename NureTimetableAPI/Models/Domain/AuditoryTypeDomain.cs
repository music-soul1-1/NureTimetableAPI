using NureTimetableAPI.Models.Dto;

namespace NureTimetableAPI.Models.Domain;

public class AuditoryTypeDomain
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int AuditoryTypeId { get; set; }

    public string Name { get; set; } = "";

    // Navigation property
    public List<AuditoryDomain> Auditories { get; set; } = [];

    public AuditoryTypeDto ToDto()
    {
        return new AuditoryTypeDto
        {
            Id = AuditoryTypeId,
            Name = Name,
        };
    }
}
