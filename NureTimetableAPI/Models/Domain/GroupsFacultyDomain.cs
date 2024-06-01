using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models.Domain;

public class GroupsFacultyDomain
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int FacultyId { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<DirectionDomain> Directions { get; set; } = [];
}
