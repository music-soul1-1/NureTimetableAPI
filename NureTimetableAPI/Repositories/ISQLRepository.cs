using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Models.Domain;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Types;

namespace NureTimetableAPI.Repositories;

public interface ISQLRepository
{
    #region University structure

    #region Fetching jobs helpers methods

    /// <summary>
    /// Performs a transaction with the database, which deletes all groups faculties and related entities, then saves new ones.
    /// </summary>
    /// <param name="p_groupsFaculties"></param>
    /// <returns></returns>
    public Task FetchGroupsFacultiesAsync(List<CistGroupsFaculty> p_groupsFaculties);

    /// <summary>
    /// Performs a transaction with the database, which deletes all teachers faculties and related entities, then saves new ones.
    /// </summary>
    /// <param name="p_teachersFaculties"></param>
    /// <returns></returns>
    public Task FetchTeachersFacultiesAsync(List<CistTeachersFaculty> p_teachersFaculties);

    /// <summary>
    /// Performs a transaction with the database, which deletes all buildings and related entities, then saves new ones.
    /// </summary>
    /// <param name="p_buildings"></param>
    /// <returns></returns>
    public Task FetchBuildingsAsync(List<CistBuilding> p_buildings);


    /// <summary>
    /// Performs a transaction with the database, which deletes lessons from DB, and saves <paramref name="cistSchedule"/> to the DB.
    /// </summary>
    /// <param name="id">
    /// Id of the entity.
    /// </param>
    /// <param name="cistSchedule">
    /// Schedule from Cist, which can be fetched by calling <see cref="ICistRepository.GetCistScheduleAsync(int, EntityType)"/>.
    /// </param>
    /// <param name="entityType">
    /// Type of schedule (group, teacher, auditory).
    /// </param>
    /// <param name="startTime">
    /// Start time of the schedule, must be UNIX Seconds timestamp.
    /// </param>
    /// <param name="endTime">
    /// End time of the schedule, must be UNIX Seconds timestamp.
    /// </param>
    /// <returns></returns>
    public Task<List<LessonDto>?> FetchSchedule(int id, CistSchedule cistSchedule, EntityType entityType, int? startTime = null, int? endTime = null);


    /// <inheritdoc cref="FetchSchedule(int, CistSchedule, EntityType, int?, int?)"/>
    /// <param name="name">
    /// Name of the entity. For teachers use name in a form of "Шевченко Т. Г.".
    /// </param>
    public Task<List<LessonDto>?> FetchSchedule(string name, CistSchedule cistSchedule, EntityType entityType, int? startTime = null, int? endTime = null);

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
    /// Gets groups faculties from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<GroupsFacultyDto>?> GetGroupsFacultiesAsync();

    /// <summary>
    /// Gets teachers faculties from the database.
    /// </summary>
    /// <returns></returns>
    public Task<List<TeachersFacultyDto>?> GetTeachersFacultiesAsync();

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

    // By ID getters

    /// <summary>
    /// Gets a group by its <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<GroupDto?> GetGroupAsync(int id);

    /// <summary>
    /// Gets a group by its <paramref name="name"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<GroupDto?> GetGroupAsync(string name);

    /// <summary>
    /// Gets a teacher by its <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<TeacherDto?> GetTeacherAsync(int id);

    /// <summary>
    /// Gets a teacher by its <paramref name="name"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<TeacherDto?> GetTeacherAsync(string name);

    /// <summary>
    /// Gets an auditory by its <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<AuditoryDto?> GetAuditoryAsync(int id);

    /// <summary>
    /// Gets an auditory by its <paramref name="name"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<AuditoryDto?> GetAuditoryAsync(string name);

    #endregion

    #endregion


    #region Schedule Getters

    public Task<List<LessonDto>?> GetLessonsAsync(int id, EntityType entityType, int? startTime = null, int? endTime = null);

    public Task<List<LessonDto>?> GetLessonsAsync(string name, EntityType entityType, int? startTime = null, int? endTime = null);

    #endregion
}
