namespace NureTimetableAPI.Models.Dto;

public class MinimalCombinedEntitiesDto
{
    public List<MinimalGroup> Groups { get; set; } = [];

    public List<MinimalTeacher> Teachers { get; set; } = [];

    public List<MinimalAuditory> Auditories { get; set; } = [];
}
