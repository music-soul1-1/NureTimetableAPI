namespace NureTimetableAPI.Models.Domain;

public class CistScheduleDomain
{
    public Guid Id { get; set; }

    public string TimeZone { get; set; } = "";

    public List<int> EventIds { get; set; } = [];

    public List<int> GroupIds { get; set; } = [];

    public List<int> SubjectIds { get; set; } = [];

    public List<int> TeacherIds { get; set; } = [];

    public List<int> TypeIds { get; set; } = [];
}
