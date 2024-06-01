using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Types;

namespace NureTimetableAPI.Repositories;

public interface ICistRepository
{
    // Structure
    public Task<List<CistGroupsFaculty>?> GetGroupsFacultiesAsync();

    public Task<List<CistTeachersFaculty>?> GetTeachersFacultiesAsync();

    public Task<List<Department>?> GetTeachersDepartments();

    public Task<List<Teacher>?> GetTeachers();

    public Task<List<Teacher>?> GetTeachers(int departmentId);

    public Task<List<CistBuilding>?> GetAuditoriesBuildingsAsync();

    public Task<List<Direction>?> GetDirections(int facultyId);

    public Task<List<CistBuilding>?> GetBuildings();

    public Task<List<CistAuditory>?> GetAuditories(string buildingId);

    public Task<List<CistAuditory>?> GetAuditories();

    public Task<List<Specialty>?> GetSpecialties(int facultyId, int directionId);

    public Task<List<Group>?> GetGroups(int facultyId, int directionId);

    public Task<List<Group>?> GetGroups();


    // Schedule for group, teacher or auditory
    public Task<CistSchedule?> GetCistScheduleAsync(int id, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null);

    public Task<CistSchedule?> GetCistScheduleAsync(string name, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null);

    public Task<List<LessonDto>?> GetLessonsAsync(int id, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null);

    public Task<List<LessonDto>?> GetLessonsAsync(string name, EntityType type = EntityType.Group, int? startTime = null, int? endTime = null);
}
