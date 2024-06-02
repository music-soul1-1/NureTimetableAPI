namespace NureTimetableAPI.Models.Dto;

public class GroupsFacultyDto
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<DirectionDto> Directions { get; set; } = [];
}
