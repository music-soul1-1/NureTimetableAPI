namespace NureTimetableAPI.Models.Dto;

public class TeachersFacultyDto
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<DepartmentDto> Departments { get; set; } = [];
}
