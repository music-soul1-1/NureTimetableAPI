namespace NureTimetableAPI.Models.Dto;

public class DirectionDto
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<MinimalGroup> Groups { get; set; } = [];
}
