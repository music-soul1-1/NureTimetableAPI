using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models.Domain;

public class DirectionDomain
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int DirectionId { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<Group> Groups { get; set; } = [];

    public Guid GroupsFacultyDomainId { get; set; } // Foreign key for GroupsFacultyDomain

    public GroupsFacultyDomain GroupsFaculty { get; set; } // Navigation property for GroupsFacultyDomain
}
