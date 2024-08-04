using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Types;

namespace NureTimetableAPI.Repositories;

public interface ICistRepository
{
    // Structure
    public Task<List<CistGroupsFaculty>?> GetGroupsFacultiesAsync();

    public Task<List<CistTeachersFaculty>?> GetTeachersFacultiesAsync();

    public Task<List<Teacher>?> GetTeachers();

    public Task<List<CistBuilding>?> GetAuditoriesBuildingsAsync();

    public Task<List<Direction>?> GetDirections(int facultyId);

    public Task<List<CistBuilding>?> GetBuildings();

    public Task<List<CistAuditory>?> GetAuditories(string buildingId);

    public Task<List<CistAuditory>?> GetAuditories();

    public Task<List<Specialty>?> GetSpecialties(int facultyId, int directionId);

    public Task<List<Group>?> GetGroups(int facultyId, int directionId);

    public Task<List<Group>?> GetGroups();


    // Schedule for group, teacher or auditory
    public Task<CistSchedule?> GetCistScheduleAsync(int id, EntityType type = EntityType.Group);

    public Task<CistSchedule?> GetCistScheduleAsync(string name, EntityType type = EntityType.Group);
}
