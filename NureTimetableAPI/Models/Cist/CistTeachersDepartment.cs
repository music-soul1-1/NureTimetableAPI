using Newtonsoft.Json;
using NureTimetableAPI.Models.Domain;

namespace NureTimetableAPI.Models.Cist;


/// <summary>
/// The difference between this and <see cref="Department"/> is that Cist can return inner departments.
/// <br>
/// This class is used in implementation of  <see cref="Repositories.ICistRepository.FetchTeachersAsync"/>
/// </br>
/// </summary>
public class CistTeachersDepartment : Department
{
    [JsonProperty("departments")]
    public List<Department> Departments { get; set; } = [];

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

    public DepartmentDomain ToDepartmentDomain()
    {
        return new DepartmentDomain
        {
            DepartmentId = Id,
            ShortName = ShortName,
            FullName = FullName,
            Teachers = Teachers,
        };
    }
}