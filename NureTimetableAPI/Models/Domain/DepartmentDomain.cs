using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models.Domain;

public class DepartmentDomain
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int DepartmentId { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<Teacher> Teachers { get; set; } = [];

    public Guid TeachersFacultyDomainId { get; set; }

    public TeachersFacultyDomain TeachersFaculty { get; set; }
}
