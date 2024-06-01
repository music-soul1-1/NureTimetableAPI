namespace NureTimetableAPI.Models.Dto;

/// <summary>
/// Represents direction, without <see cref="Group"/> and <see cref="Specialty"/>.
/// </summary>
public class MinimalDirection
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";
}
