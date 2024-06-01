using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models.Domain;

public class TeachersFacultyDomain
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int FacultyId { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<DepartmentDomain> Departments { get; set; } = [];
}
