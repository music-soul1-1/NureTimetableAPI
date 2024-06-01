namespace NureTimetableAPI.Models.Dto;

public class TeacherDto
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public Entity Department { get; set; } = new Entity();

    public Entity Faculty { get; set; } = new Entity();
}
