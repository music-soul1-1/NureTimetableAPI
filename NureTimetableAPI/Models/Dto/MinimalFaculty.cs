namespace NureTimetableAPI.Models.Dto;

/// <summary>
/// Represents faculty, without <see cref="Direction"/>.
/// </summary>
public class MinimalFaculty
{
    public int Id { get; set; }

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";
}
