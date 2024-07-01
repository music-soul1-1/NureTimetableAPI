using NureTimetableAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace NureTimetableAPI.Models.Domain;

public class LessonDomain
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public List<Guid> ScheduleLogIds { get; set; } = [];

    public int LessonId { get; set; }

    public string Brief { get; set; } = "";

    public string Title { get; set; } = "";

    public int StartTime { get; set; }

    public int EndTime { get; set; }

    public Guid TypeId { get; set; }

    public LessonType Type { get; set; } = new LessonType();

    public int NumberPair { get; set; }

    [Required]
    public List<Guid> TeacherIds { get; set; } = [];

    public List<Teacher> Teachers { get; set; } = [];

    public Guid AuditoryId { get; set; }

    public AuditoryDomain Auditory { get; set; } = new AuditoryDomain();

    [Required]
    public List<Guid> GroupIds { get; set; } = [];

    public List<Group> Groups { get; set; } = [];

    public LessonDto ToLessonDto()
    {
        return new LessonDto
        {
            Id = LessonId,
            Brief = Brief,
            Title = Title,
            StartTime = StartTime,
            EndTime = EndTime,
            Type = Type.ToDto(),
            NumberPair = NumberPair,
            Teachers = Teachers.Select(t => new TeacherDto
            {
                Id = t.TeacherId,
                ShortName = t.ShortName,
                FullName = t.FullName,
                Faculty = new Entity
                {
                    Id = t.Department.TeachersFaculty.FacultyId,
                    ShortName = t.Department.TeachersFaculty.ShortName,
                    FullName = t.Department.TeachersFaculty.FullName,
                },
                Department = new Entity
                {
                    Id = t.Department.DepartmentId,
                    ShortName = t.Department.ShortName,
                    FullName = t.Department.FullName,
                },
            }).ToList(),
            Auditory = new AuditoryDto
            {
                Id = Auditory.AuditoryId,
                Name = Auditory.ShortName,
                Floor = Auditory.Floor ?? 0,
                HasPower = Auditory.HasPower,
                Building = new MinimalBuilding
                {
                    Id = Auditory.Building.BuildingId,
                    ShortName = Auditory.Building.ShortName,
                    FullName = Auditory.Building.FullName,
                },
                AuditoryTypes = Auditory.AuditoryTypes.Select(at => new AuditoryTypeDto
                {
                    Id = at.AuditoryTypeId,
                    Name = at.Name,
                }).ToList(),
            },
            Groups = Groups.Select(g => new GroupDto
            {
                Id = g.GroupId,
                Name = g.Name,
                Direction = new Entity
                {
                    Id = g.Direction.DirectionId,
                    ShortName = g.Direction.ShortName,
                    FullName = g.Direction.FullName,
                },
                Faculty = new Entity
                {
                    Id = g.Direction.GroupsFaculty.FacultyId,
                    ShortName = g.Direction.GroupsFaculty.ShortName,
                    FullName = g.Direction.GroupsFaculty.FullName,
                },
            }).ToList(),
        };
    }
}
