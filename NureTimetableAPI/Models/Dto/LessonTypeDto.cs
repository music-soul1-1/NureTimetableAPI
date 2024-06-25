namespace NureTimetableAPI.Models.Dto;

public class LessonTypeDto
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public int IdBase { get; set; }

    public string Type { get; set; } = "";
}
