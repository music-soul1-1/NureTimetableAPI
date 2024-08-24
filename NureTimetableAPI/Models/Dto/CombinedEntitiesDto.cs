namespace NureTimetableAPI.Models.Dto;

public class CombinedEntitiesDto
{
    public List<GroupDto> Groups { get; set; } = [];

    public List<TeacherDto> Teachers { get; set; } = [];

    public List<AuditoryDto> Auditories { get; set; } = [];
}
