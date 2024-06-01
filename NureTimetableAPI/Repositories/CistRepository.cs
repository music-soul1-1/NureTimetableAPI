using Newtonsoft.Json;
using NureTimetableAPI.Helpers;
using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Types;

namespace NureTimetableAPI.Repositories;

public class CistRepository() : ICistRepository
{
    private static readonly HttpClient httpClient = new();

    /// <summary>
    /// Used to store groups faculties.
    /// <br>
    /// Note: this variable is used to temporarily store university structure, 
    /// so that it's not fetched from CIST every time.
    /// </br>
    /// </summary>
    private List<CistGroupsFaculty>? groupsFaculties;

    private List<CistTeachersFaculty>? teachersFaculties;

    /// <summary>
    /// Used to store university departments.
    /// <br>
    /// Note: this variable is used to temporarily store university structure, 
    /// so that it's not fetched from CIST every time.
    /// </br>
    /// </summary>
    private List<Department>? departments;

    /// <summary>
    /// Used to store university buildings.
    /// <br>
    /// Note: this variable is used to temporarily store university structure, 
    /// so that it's not fetched from CIST every time.
    /// </br>
    /// </summary>
    private List<CistBuilding>? buildings;


    #region Cist Structure Getters (directly from CIST)

    public async Task<List<CistGroupsFaculty>?> GetGroupsFacultiesAsync()
    {
        var response = await httpClient.GetAsync("https://cist.nure.ua/ias/app/tt/P_API_GROUP_JSON");
        var cistResponse = await HttpResponseDecoder.DeserializeResponse<CistGroupStructureResponse>(response);

        groupsFaculties = cistResponse?.University.Faculties;

        return groupsFaculties;
    }

    public async Task<List<CistBuilding>?> GetAuditoriesBuildingsAsync()
    {
        var response = await httpClient.GetAsync("https://cist.nure.ua/ias/app/tt/P_API_AUDITORIES_JSON");
        var cistResponse = await HttpResponseDecoder.DeserializeResponse<CistAuditoriesStructureResponse>(response);

        buildings = cistResponse?.University.Buildings;

        return buildings;
    }

    public async Task<List<CistTeachersFaculty>?> GetTeachersFacultiesAsync()
    {
        if (teachersFaculties == null)
        {
            var response = await httpClient.GetAsync("https://cist.nure.ua/ias/app/tt/P_API_PODR_JSON");
            var jsonString = await HttpResponseDecoder.ConvertToString(response);

            // Fixing Cist JSON
            jsonString = jsonString.Trim().Remove(jsonString.Length - 2);
            jsonString += "]}}";

            var data = JsonConvert.DeserializeObject<CistTeachersStructureResponse>(jsonString);

            if (data == null)
            {
                return null;
            }

            teachersFaculties = data.University.Faculties;

            return data.University.Faculties;
        }
        return teachersFaculties;
    }

    public async Task<List<Department>?> GetTeachersDepartments()
    {
        if (departments == null)
        {
            var response = await httpClient.GetAsync("https://cist.nure.ua/ias/app/tt/P_API_PODR_JSON");
            var jsonString = await HttpResponseDecoder.ConvertToString(response);

            // Fixing Cist JSON
            jsonString = jsonString.Trim().Remove(jsonString.Length - 2);
            jsonString += "]}}";

            var data = JsonConvert.DeserializeObject<CistTeachersStructureResponse>(jsonString);
            List<Department>? departments = [];

            if (data == null)
            {
                return null;
            }

            foreach (var faculty in data.University.Faculties)
            {
                foreach (var department in faculty.Departments)
                {
                    if (department.Teachers.Count > 0)
                    {
                        departments.Add(department.ToDepartment());
                    }
                    else
                    {
                        foreach (var innerDepartment in department.Departments)
                        {
                            departments.Add(innerDepartment);
                        }
                    }
                }
            }
            this.departments = departments;
        }

        return departments;
    }

    #endregion

    #region Additional Getters

    public async Task<List<Direction>?> GetDirections(int facultyId)
    {
        if (groupsFaculties == null)
        {
            await GetGroupsFacultiesAsync();
        }

        return groupsFaculties?.FirstOrDefault(f => f.Id == facultyId)?.Directions;
    }

    public async Task<List<Specialty>?> GetSpecialties(int facultyId, int directionId)
    {
        if (groupsFaculties == null)
        {
            await GetGroupsFacultiesAsync();
        }

        return groupsFaculties?
            .FirstOrDefault(f => f.Id == facultyId)?
            .Directions.FirstOrDefault(d => d.Id == directionId)?
            .Specialties;
    }

    public async Task<List<Group>?> GetGroups()
    {
        var groups = new List<Group>();

        if (groupsFaculties == null)
        {
            await GetGroupsFacultiesAsync();
        }

        if (groupsFaculties != null)
        {
            foreach (var faculty in groupsFaculties)
            {
                foreach (var direction in faculty.Directions)
                {
                    // This is needed because CIST may have groups or specialties array empty
                    if (direction.Groups.Count > 0)
                    {
                        groups.AddRange(direction.Groups);
                    }
                    else if (direction.Specialties.Count > 0)
                    {
                        groups.AddRange(direction.Specialties.SelectMany(specialty => specialty.Groups));
                    }
                }
            }
        }

        return groups;
    }

    public async Task<List<Group>?> GetGroups(int facultyId, int directionId)
    {
        if (groupsFaculties == null)
        {
            await GetGroupsFacultiesAsync();
        }

        var groups = new List<Group>();

        if (groupsFaculties != null)
        {
            var faculty = groupsFaculties.FirstOrDefault(f => f.Id == facultyId);

            if (faculty != null)
            {
                var direction = faculty.Directions.FirstOrDefault(d => d.Id == directionId);

                if (direction != null)
                {
                    if (direction.Groups.Count > 0)
                    {
                        groups.AddRange(direction.Groups);
                    }
                    else
                    {
                        foreach (var specialty in direction.Specialties)
                        {
                            groups.AddRange(specialty.Groups);
                        }
                    }
                }
            }
        }

        return groups;
    }


    public async Task<List<Teacher>?> GetTeachers()
    {
        if (teachersFaculties == null)
        {
            await GetTeachersFacultiesAsync();
        }

        var teachers = new List<Teacher>();

        if (teachersFaculties != null)
        {
            teachers.AddRange(teachersFaculties
                .SelectMany(f => f.Departments)
                .SelectMany(d => d.Teachers));

            teachers.AddRange(teachersFaculties
                .SelectMany(f => f.Departments)
                .SelectMany(d => d.Departments)
                .SelectMany(d => d.Teachers));
        }
        
        return teachers;
    }

    public async Task<List<Teacher>?> GetTeachers(int departmentId)
    {
        if (teachersFaculties == null)
        {
            await GetTeachersFacultiesAsync();
        }
        
        var teachers = new List<Teacher>();

        if (teachersFaculties != null)
        {
            if (teachersFaculties.Any(f => f.Departments.Any(d => d.Id == departmentId)))
            {
                teachers.AddRange(teachersFaculties
                    .SelectMany(f => f.Departments)
                    .First(d => d.Id == departmentId)
                    .Teachers);
            }

            if (teachersFaculties.Any(f => f.Departments.Any(d => d.Departments.Any(d => d.Id == departmentId))))
            {
                teachers.AddRange(teachersFaculties
                    .SelectMany(f => f.Departments)
                    .SelectMany(d => d.Departments)
                    .First(d => d.Id == departmentId)
                    .Teachers);
            }
        }

        return teachers;
    }


    public async Task<List<CistBuilding>?> GetBuildings()
    {
        if (buildings == null)
        {
            await GetAuditoriesBuildingsAsync();
        }

        return buildings;
    }

    public async Task<List<CistAuditory>?> GetAuditories()
    {
        if (buildings == null)
        {
            await GetAuditoriesBuildingsAsync();
        }

        return buildings?
            .SelectMany(b => b.Auditories)
            .ToList();
    }

    public async Task<List<CistAuditory>?> GetAuditories(string buildingId)
    {
        if (buildings == null)
        {
            await GetAuditoriesBuildingsAsync();
        }

        return buildings?
            .FirstOrDefault(b => b.Id == buildingId)?
            .Auditories;
    }

    #endregion


    #region Schedule for group or teacher

    public async Task<CistSchedule?> GetCistScheduleAsync(int id, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null)
    {
        var response = await httpClient.GetAsync(
            $"https://cist.nure.ua/ias/app/tt/P_API_EVENT_JSON?timetable_id={id}" +
            $"&type_id={
                type switch 
                { 
                    EntityType.Group => 1, 
                    EntityType.Teacher => 2, 
                    EntityType.Auditory => 3, 
                    _ => 1 
                }
                }" +
            $"&idClient=KNURESked"
            );
        
        return await HttpResponseDecoder.DeserializeResponse<CistSchedule>(response);
    }

    public async Task<CistSchedule?> GetCistScheduleAsync(string name, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null)
    {
        switch(type)
        {
            case EntityType.Group:
                var groups = await GetGroups();
                var groupId = groups?.FirstOrDefault(g => g.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.GroupId;

                if (groupId == null)
                {
                    Console.WriteLine($"Group with name {name} not found");
                    return null;
                }

                return await GetCistScheduleAsync(groupId.Value, type, startTime, endTime);

            case EntityType.Teacher:
                var teachers = await GetTeachers();
                var teacherId = teachers?.FirstOrDefault(t => t.FullName.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.TeacherId;

                if (teacherId == null)
                {
                    Console.WriteLine($"Teacher with name {name} not found");
                    return null;
                }

                return await GetCistScheduleAsync(teacherId.Value, type, startTime, endTime);

            case EntityType.Auditory:
                var auditories = await GetAuditories();
                var auditoryId = auditories?.FirstOrDefault(a => a.ShortName.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.Id;

                if (auditoryId == null)
                {
                    Console.WriteLine($"Auditory with name {name} not found");
                    return null;
                }

                return await GetCistScheduleAsync(auditoryId.Value, type, startTime, endTime);
        }

        return null;
    }

    public async Task<List<LessonDto>?> GetLessonsAsync(int id, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null)
    {
        var cistSchedule = await GetCistScheduleAsync(id, type, startTime, endTime);

        if (cistSchedule == null)
        {
            Console.WriteLine($"CistSchedule for group with id {id} not found");
            return null;
        }

        return ConvertCistScheduleToLesson(cistSchedule);
    }

    public async Task<List<LessonDto>?> GetLessonsAsync(string name, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null)
    {
        var cistSchedule = await GetCistScheduleAsync(name, type, startTime, endTime);

        if (cistSchedule == null)
        {
            Console.WriteLine($"CistSchedule for group with name {name} not found");
            return null;
        }

        return ConvertCistScheduleToLesson(cistSchedule);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Gets teachers from CIST.
    /// </summary>
    /// <returns></returns>
    private static async Task<List<Teacher>?> FetchTeachersAsync()
    {
        var response = await httpClient.GetAsync("https://cist.nure.ua/ias/app/tt/P_API_PODR_JSON");
        var jsonString = await HttpResponseDecoder.ConvertToString(response);

        // Fixing Cist JSON
        jsonString = jsonString.Remove(jsonString.Length - 2);
        jsonString += "]}}";

        var data = JsonConvert.DeserializeObject<CistTeachersStructureResponse>(jsonString);
        List<Teacher>? teachers = [];

        if (data == null)
        {
            return null;
        }

        foreach (var faculty in data.University.Faculties)
        {
            foreach (var department in faculty.Departments)
            {
                if (department.Teachers.Count > 0)
                {
                    teachers.AddRange(department.Teachers);
                }
                else
                {
                    foreach (var innerDepartment in department.Departments)
                    {
                        teachers.AddRange(innerDepartment.Teachers);
                    }
                }
            }
        }

        return teachers;
    }

    private static List<LessonDto>? ConvertCistScheduleToLesson(CistSchedule cistSchedule)
    {
        var lessons = new List<LessonDto>();

        foreach (var _event in cistSchedule.Events)
        {
            lessons.Add(new LessonDto
            {
                Id = _event.SubjectId,
                Auditory = _event.Auditory,
                Brief = cistSchedule.Subjects.Find(subject => subject.Id == _event.SubjectId)?.Brief,
                Title = cistSchedule.Subjects.Find(subject => subject.Id == _event.SubjectId)?.Title,
                Type = cistSchedule.Types.Find(type => _event.Type == type.Id),
                NumberPair = _event.NumberPair,
                StartTime = _event.StartTime,
                EndTime = _event.EndTime,
                Teachers = cistSchedule.Teachers.Where(teacher => _event.TeacherIds.Contains(teacher.TeacherId)).ToList(),
                Groups = cistSchedule.Groups.Where(group => _event.GroupIds.Contains(group.GroupId)).ToList(),
            });
        }
        lessons.Sort((lesson1, lesson2) => lesson1.StartTime.CompareTo(lesson2.StartTime));

        return lessons;
    }

    #endregion
}
