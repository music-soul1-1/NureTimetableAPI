﻿namespace NureTimetableAPI.Models.Dto;

public class BuildingDto
{
    public string Id { get; set; } = "";

    public string ShortName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<MinimalAuditory> Auditories { get; set; } = [];
}
