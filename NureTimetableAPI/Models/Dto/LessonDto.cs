namespace NureTimetableAPI.Models.Dto;

public class LessonDto
{
    public int Id { get; set; }

    public string Brief { get; set; } = "";

    public string Title { get; set; } = "";

    public int StartTime { get; set; }

    public int EndTime { get; set; }

    public LessonType Type { get; set; } = new LessonType();

    public int NumberPair { get; set; }

    public List<Teacher> Teachers { get; set; } = new List<Teacher>();

    public string Auditory { get; set; } = "";

    public List<Group> Groups { get; set; } = new List<Group>();
}
