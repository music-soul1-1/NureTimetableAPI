namespace NureTimetableAPI.Models.Dto;

public class LessonDto
{
    public int Id { get; set; }

    public string Brief { get; set; } = "";

    public string Title { get; set; } = "";

    public int StartTime { get; set; }

    public int EndTime { get; set; }

    public LessonTypeDto Type { get; set; } = new LessonTypeDto();

    public int NumberPair { get; set; }

    public List<TeacherDto> Teachers { get; set; } = [];

    public AuditoryDto Auditory { get; set; } = new AuditoryDto();

    public List<GroupDto> Groups { get; set; } = [];
}
