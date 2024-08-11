using Newtonsoft.Json;
using NureTimetableAPI.Exceptions;
using NureTimetableAPI.Helpers;
using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Types;
using System.Net;

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

    /// <summary>
    /// Used to store teachers faculties.
    /// <br>
    /// Note: this variable is used to temporarily store university structure,
    /// so that it's not fetched from CIST every time.
    /// </br>
    /// </summary>
    private List<CistTeachersFaculty>? teachersFaculties;

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

        if (cistResponse == null)
        {
            return null;
        }

        groupsFaculties = cistResponse?.University.Faculties;

        return groupsFaculties;
    }

    public async Task<List<CistBuilding>?> GetAuditoriesBuildingsAsync()
    {
        var response = await httpClient.GetAsync("https://cist.nure.ua/ias/app/tt/P_API_AUDITORIES_JSON");
        var cistResponse = await HttpResponseDecoder.DeserializeResponse<CistAuditoriesStructureResponse>(response);

        if (cistResponse == null)
        {
            return null;
        }

        buildings = cistResponse.University.Buildings;

        return buildings;
    }

    public async Task<List<CistTeachersFaculty>?> GetTeachersFacultiesAsync()
    {
        var response = await httpClient.GetAsync("https://cist.nure.ua/ias/app/tt/P_API_PODR_JSON");

        var data = await HttpResponseDecoder.DeserializeResponse<CistTeachersStructureResponse>(response);

        if (data == null)
        {
            return null;
        }

        teachersFaculties = data.University.Faculties;

        return data.University.Faculties;
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
                .SelectMany(f => f.InnerFaculties)
                .SelectMany(d => d.Teachers));

            teachers.AddRange(teachersFaculties
                .SelectMany(f => f.InnerFaculties)
                .SelectMany(d => d.InnerInnerFaculties)
                .SelectMany(d => d.Teachers));

            teachers.AddRange(teachersFaculties
                .SelectMany(f => f.InnerFaculties)
                .SelectMany(d => d.InnerInnerFaculties)
                .SelectMany(d => d.Departments)
                .SelectMany(d => d.Teachers));
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


    #region Schedule for group, teacher or auditory

    public async Task<CistSchedule?> GetCistScheduleAsync(int id, EntityType type = EntityType.Group)
    {
        var response = await httpClient.GetAsync(
            $"https://cist.nure.ua/ias/app/tt/P_API_EVENT_JSON?timetable_id={id}" +
            $"&type_id={type switch
            {
                EntityType.Group => 1,
                EntityType.Teacher => 2,
                EntityType.Auditory => 3,
                _ => 1
            }}" +
            $"&idClient=KNURESked"
            );

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"Schedule for {type} with id {id} not found");
        }

        return await HttpResponseDecoder.DeserializeResponse<CistSchedule>(response);
    }

    public async Task<CistSchedule?> GetCistScheduleAsync(string name, EntityType type = EntityType.Group)
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

                return await GetCistScheduleAsync(groupId.Value, type);

            case EntityType.Teacher:
                var teachers = await GetTeachers();
                var teacherId = teachers?.FirstOrDefault(t => t.FullName.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.TeacherId;

                if (teacherId == null)
                {
                    Console.WriteLine($"Teacher with name {name} not found");
                    return null;
                }

                return await GetCistScheduleAsync(teacherId.Value, type);

            case EntityType.Auditory:
                var auditories = await GetAuditories();
                var auditoryId = auditories?.FirstOrDefault(a => a.ShortName.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.Id;

                if (auditoryId == null)
                {
                    Console.WriteLine($"Auditory with name {name} not found");
                    return null;
                }

                return await GetCistScheduleAsync(auditoryId.Value, type);
        }

        return null;
    }

    #endregion
}
