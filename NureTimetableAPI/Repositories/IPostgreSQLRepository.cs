using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Models.Domain;
using NureTimetableAPI.Models.Dto;

namespace NureTimetableAPI.Repositories;

public interface IPostgreSQLRepository
{
    #region University structure

    #region Fetching jobs helpers methods

    /// <summary>
    /// Saves groups faculties to the database.
    /// </summary>
    /// <param name="p_groupsFaculties"></param>
    /// <returns></returns>
    public Task<List<GroupsFacultyDomain>?> SaveGroupsFacultiesToDbAsync(List<CistGroupsFaculty> p_groupsFaculties);

    /// <summary>
    /// Saves teachers faculties to the database.
    /// </summary>
    /// <param name="p_teachersFaculties"></param>
    /// <returns></returns>
    public Task<List<TeachersFacultyDomain>?> SaveTeachersFacultiesToDbAsync(List<CistTeachersFaculty> p_teachersFaculties);

    /// <summary>
    /// Saves buildings to the database.
    /// </summary>
    /// <param name="p_buildings"></param>
    /// <returns></returns>
    public Task<List<BuildingDomain>?> SaveBuildingsToDbAsync(List<CistBuilding> p_buildings);

    /// <summary>
    /// Performs a transaction with the database, which deletes all groups faculties and related entities, then saves new ones.
    /// </summary>
    /// <param name="p_groupsFaculties"></param>
    /// <returns></returns>
    public Task ClearAndSaveGroupsFacultiesAsync(List<CistGroupsFaculty> p_groupsFaculties);

    /// <summary>
    /// Performs a transaction with the database, which deletes all teachers faculties and related entities, then saves new ones.
    /// </summary>
    /// <param name="p_teachersFaculties"></param>
    /// <returns></returns>
    public Task ClearAndSaveTeachersFacultiesAsync(List<CistTeachersFaculty> p_teachersFaculties);

    /// <summary>
    /// Performs a transaction with the database, which deletes all buildings and related entities, then saves new ones.
    /// </summary>
    /// <param name="p_buildings"></param>
    /// <returns></returns>
    public Task ClearAndSaveBuildingsAsync(List<CistBuilding> p_buildings);

    #endregion

    #region Domain models getters

    /// <summary>
    /// Gets groups faculties from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<GroupsFacultyDomain>?> GetGroupsFacultyDomainsAsync();

    /// <summary>
    /// Gets teachers faculties from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<TeachersFacultyDomain>?> GetTeachersFacultyDomainsAsync();

    /// <summary>
    /// Gets buildings from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<BuildingDomain>?> GetBuildingDomainsAsync();

    #endregion

    #region Dto models getters

    /// <summary>
    /// Gets buildings from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<BuildingDto>?> GetBuildingsAsync();

    /// <summary>
    /// Gets groups from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<GroupDto>?> GetGroupsAsync();

    /// <summary>
    /// Gets teachers from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<TeacherDto>?> GetTeachersAsync();

    /// <summary>
    /// Gets auditories from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<AuditoryDto>?> GetAuditoriesAsync();

    #endregion

    #endregion

    #region Schedule

    public Task<CistSchedule> GetCistScheduleAsync(int groupId, int? startTime = null, int? endTime = null);

    public Task<CistSchedule> GetCistScheduleAsync(string groupName, int? startTime = null, int? endTime = null);

    public Task<List<LessonDto>> GetLessonsAsync(int groupId, int? startTime = null, int? endTime = null);

    public Task<List<LessonDto>> GetLessonsAsync(string groupName, int? startTime = null, int? endTime = null);

    #endregion
}
