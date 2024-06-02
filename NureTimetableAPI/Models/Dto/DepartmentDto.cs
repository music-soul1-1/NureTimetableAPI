namespace NureTimetableAPI.Models.Dto;

public class DepartmentDto
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<MinimalTeacher> Teachers { get; set; } = [];
}
