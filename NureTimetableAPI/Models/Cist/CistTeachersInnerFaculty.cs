using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;

namespace NureTimetableAPI.Models.Cist;

public class CistTeachersInnerFaculty : Department
{
    [JsonProperty("departments")]
    public List<CistTeachersInnerInnerFaculty> InnerInnerFaculties { get; set; } = [];

    public Department ToDepartment()
    {
        return new Department
        {
            Id = Id,
            ShortName = ShortName,
            FullName = FullName,
            Teachers = Teachers,
        };
    }

    public DepartmentDomain ToDepartmentDomain(TeachersFacultyDomain teachersFacultyDomain)
    {
        return new DepartmentDomain
        {
            DepartmentId = Id,
            ShortName = ShortName,
            FullName = FullName,
            Teachers = Teachers,
            TeachersFaculty = teachersFacultyDomain,
            TeachersFacultyDomainId = teachersFacultyDomain.Id,
        };
    }
}
