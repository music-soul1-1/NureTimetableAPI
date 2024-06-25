using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using NureTimetableAPI.Contexts;
using NureTimetableAPI.Jobs;
using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Models.Domain;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Types;

namespace NureTimetableAPI.Repositories;

public class SQLRepository(NureTimetableDbContext dbContext) : ISQLRepository
{
    private readonly NureTimetableDbContext _dbContext = dbContext;

    #region Fetching jobs methods

    public async Task FetchGroupsFacultiesAsync(List<CistGroupsFaculty> p_groupsFaculties)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            ArgumentNullException.ThrowIfNull(p_groupsFaculties, nameof(p_groupsFaculties));

            await DeleteAllGroupsFacultiesFromDb();
            await SaveGroupsFacultiesToDbAsync(p_groupsFaculties);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task FetchTeachersFacultiesAsync(List<CistTeachersFaculty> p_teachersFaculties)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            ArgumentNullException.ThrowIfNull(p_teachersFaculties, nameof(p_teachersFaculties));

            await DeleteAllTeachersFacultiesFromDb();

            await SaveTeachersFacultiesToDbAsync(p_teachersFaculties);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task FetchBuildingsAsync(List<CistBuilding> p_buildings)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            ArgumentNullException.ThrowIfNull(p_buildings, nameof(p_buildings));

            await DeleteAllBuildingsFromDb();
            await SaveBuildingsToDbAsync(p_buildings);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<LessonDto>?> FetchSchedule(int id, CistSchedule cistSchedule, EntityType entityType, int? startTime = null, int? endTime = null)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            ArgumentNullException.ThrowIfNull(cistSchedule, nameof(cistSchedule));

            await DeleteLessonsFromDbAsync(id, entityType);
            var lessons = await SaveLessonsToDbAsync(id, cistSchedule, entityType);

            await transaction.CommitAsync();

            if (lessons == null)
            {
                return null;
            }

            lessons.RemoveAll(l => startTime != null && l.StartTime <= startTime);
            lessons.RemoveAll(l => endTime != null && l.EndTime >= endTime);

            return lessons;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error while fetching schedule:\n {ex.Message}");

            return null;
        }
    }

    public async Task<List<LessonDto>?> FetchSchedule(string name, CistSchedule cistSchedule, EntityType entityType, int? startTime = null, int? endTime = null)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            ArgumentNullException.ThrowIfNull(cistSchedule, nameof(cistSchedule));

            var id = entityType switch
            {
                EntityType.Group => await _dbContext.Groups.Where(g => g.Name == name).Select(g => g.GroupId).FirstOrDefaultAsync(),
                EntityType.Teacher => await _dbContext.Teachers.Where(t => t.ShortName == name).Select(t => t.TeacherId).FirstOrDefaultAsync(),
                EntityType.Auditory => await _dbContext.Auditories.Where(a => a.ShortName == name).Select(a => a.AuditoryId).FirstOrDefaultAsync(),
                _ => throw new ArgumentException($"Invalid entity type: {entityType}"),
            };

            await DeleteLessonsFromDbAsync(id, entityType);
            var lessons = await SaveLessonsToDbAsync(id, cistSchedule, entityType);

            await transaction.CommitAsync();

            if (lessons == null)
            {
                return null;
            }

            lessons.RemoveAll(l => startTime != null && l.StartTime <= startTime);
            lessons.RemoveAll(l => endTime != null && l.EndTime >= endTime);

            return lessons;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error while fetching schedule for {entityType} with name {name}:\n {ex.Message}");

            return null;
        }
    }

    #endregion

    #region Domain models getters

    public async Task<List<GroupsFacultyDomain>?> GetGroupsFacultyDomainsAsync()
    {
        return await _dbContext.GroupsFaculty
            .Include(f => f.Directions)
                .ThenInclude(d => d.Groups)
            .ToListAsync();
    }

    public async Task<List<TeachersFacultyDomain>?> GetTeachersFacultyDomainsAsync()
    {
        return await _dbContext.TeachersFaculties
            .Include(f => f.Departments)
            .ThenInclude(d => d.Teachers)
            .ToListAsync();
    }

    public async Task<List<BuildingDomain>?> GetBuildingDomainsAsync()
    {
        return await _dbContext.Buildings
            .Include(b => b.Auditories)
                .ThenInclude(a => a.AuditoryTypes)
            .ToListAsync();
    }

    #endregion

    #region DTO models getters

    #region Uni structure

    public async Task<List<GroupsFacultyDto>?> GetGroupsFacultiesAsync()
    {
        var faculties = await _dbContext.GroupsFaculty
            .Include(f => f.Directions)
                .ThenInclude(d => d.Groups)
            .ToListAsync();

        if (faculties == null)
        {
            return null;
        }

        var facultiesDto = new List<GroupsFacultyDto>();

        foreach (var faculty in faculties)
        {
            var directions = new List<DirectionDto>();

            foreach (var direction in faculty.Directions)
            {
                directions.Add(new DirectionDto
                {
                    Id = direction.DirectionId,
                    ShortName = direction.ShortName,
                    FullName = direction.FullName,
                    Groups = direction.Groups.Select(g => new MinimalGroup
                    {
                        Id = g.GroupId,
                        Name = g.Name,
                    }).ToList(),
                });
            }

            facultiesDto.Add(new GroupsFacultyDto
            {
                Id = faculty.FacultyId,
                ShortName = faculty.ShortName,
                FullName = faculty.FullName,
                Directions = directions,
            });
        }

        return facultiesDto;
    }

    public async Task<List<TeachersFacultyDto>?> GetTeachersFacultiesAsync()
    {
        var faculties = await _dbContext.TeachersFaculties
            .Include(f => f.Departments)
            .ThenInclude(d => d.Teachers)
            .ToListAsync();

        if (faculties == null)
        {
            return null;
        }

        var facultiesDto = new List<TeachersFacultyDto>();

        foreach (var faculty in faculties)
        {
            var departments = new List<DepartmentDto>();

            foreach (var department in faculty.Departments)
            {
                departments.Add(new DepartmentDto
                {
                    Id = department.DepartmentId,
                    ShortName = department.ShortName,
                    FullName = department.FullName,
                    Teachers = department.Teachers.Select(t => new MinimalTeacher
                    {
                        Id = t.TeacherId,
                        ShortName = t.ShortName,
                        FullName = t.FullName,
                    }).ToList(),
                });
            }

            facultiesDto.Add(new TeachersFacultyDto
            {
                Id = faculty.FacultyId,
                ShortName = faculty.ShortName,
                FullName = faculty.FullName,
                Departments = departments,
            });
        }

        return facultiesDto;
    }

    public async Task<List<GroupDto>?> GetGroupsAsync()
    {
        var faculties = await _dbContext.GroupsFaculty
            .Include(f => f.Directions)
                .ThenInclude(d => d.Groups)
            .ToListAsync();

        if (faculties == null)
        {
            return null;
        }

        var groups = new List<GroupDto>();

        foreach (var facultyDomain in faculties)
        {
            foreach (var directionDomain in facultyDomain.Directions)
            {
                foreach (var groupDomain in directionDomain.Groups)
                {
                    groups.Add(new GroupDto
                    {
                        Id = groupDomain.GroupId,
                        Name = groupDomain.Name,
                        Direction = new Entity
                        {
                            Id = directionDomain.DirectionId,
                            ShortName = directionDomain.ShortName,
                            FullName = directionDomain.FullName,
                        },
                        Faculty = new Entity
                        {
                            Id = facultyDomain.FacultyId,
                            ShortName = facultyDomain.ShortName,
                            FullName = facultyDomain.FullName,
                        },
                    });
                }
            }
        }

        return groups;
    }

    public async Task<List<TeacherDto>?> GetTeachersAsync()
    {
        var faculties = await _dbContext.TeachersFaculties
            .Include(f => f.Departments)
            .ThenInclude(d => d.Teachers)
            .ToListAsync();

        if (faculties == null)
        {
           return null;
        }

        var teachers = new List<TeacherDto>();

        foreach (var faculty in faculties)
        {
            foreach (var department in faculty.Departments)
            {
                foreach (var teacher in department.Teachers)
                {
                    teachers.Add(new TeacherDto
                    {
                        Id = teacher.TeacherId,
                        ShortName = teacher.ShortName,
                        FullName = teacher.FullName,
                        Faculty = new Entity
                        {
                            Id = faculty.FacultyId,
                            ShortName = faculty.ShortName,
                            FullName = faculty.FullName,
                        },
                        Department = new Entity
                        {
                            Id = department.DepartmentId,
                            ShortName = department.ShortName,
                            FullName = department.FullName,
                        },
                    });
                }
            }
        }

        return teachers;
    }

    public async Task<List<BuildingDto>?> GetBuildingsAsync()
    {
        var buildings = await _dbContext.Buildings
            .Include(b => b.Auditories)
                .ThenInclude(a => a.AuditoryTypes)
            .ToListAsync();

        if (buildings == null)
        {
           return null;
        }

        var buildingsDto = new List<BuildingDto>();

        foreach (var building in buildings)
        {
            buildingsDto.Add(new BuildingDto
            {
                Id = building.BuildingId,
                ShortName = building.ShortName,
                FullName = building.FullName,
                Auditories = building.Auditories.Select(a => new MinimalAuditory
                {
                    Id = a.AuditoryId,
                    Name = a.ShortName,
                    Floor = a.Floor,
                    HasPower = a.HasPower,
                    AuditoryTypes = a.AuditoryTypes.Select(at => new AuditoryTypeDto
                    {
                        Id = at.AuditoryTypeId,
                        Name = at.Name,
                    }).ToList(),
                }).ToList(),
            });
        }

        return buildingsDto;
    }

    public async Task<List<AuditoryDto>?> GetAuditoriesAsync()
    {
        var buildings = await _dbContext.Buildings
            .Include(b => b.Auditories)
                .ThenInclude(a => a.AuditoryTypes)
            .ToListAsync();

        if (buildings == null)
        {
            return null;
        }

        var auditories = new List<AuditoryDto>();

        foreach (var building in buildings)
        {
            foreach (var auditory in building.Auditories)
            {
                auditories.Add(new AuditoryDto
                {
                    Id = auditory.AuditoryId,
                    Name = auditory.ShortName,
                    Floor = auditory.Floor,
                    HasPower = auditory.HasPower,
                    Building = new MinimalBuilding
                    {
                        Id = building.BuildingId,
                        ShortName = building.ShortName,
                        FullName = building.FullName,
                    },
                    AuditoryTypes = auditory.AuditoryTypes.Select(at => new AuditoryTypeDto
                    {
                        Id = at.AuditoryTypeId,
                        Name = at.Name,
                    }).ToList(),
                });
            }
        }

        return auditories;
    }

    public async Task<GroupDto?> GetGroupAsync(int id)
    {
       var group = await _dbContext.Groups.Where(g => g.GroupId == id)
           .Include(g => g.Direction)
           .ThenInclude(d => d.GroupsFaculty)
           .FirstOrDefaultAsync();

        if (group == null)
        {
            return null;
        }

        return new GroupDto
        {
            Id = group.GroupId,
            Name = group.Name,
            Direction = new Entity
            {
                Id = group.Direction.DirectionId,
                ShortName = group.Direction.ShortName,
                FullName = group.Direction.FullName,
            },
            Faculty = new Entity
            {
                Id = group.Direction.GroupsFaculty.FacultyId,
                ShortName = group.Direction.GroupsFaculty.ShortName,
                FullName = group.Direction.GroupsFaculty.FullName,
            },
        };
    }

    public async Task<TeacherDto?> GetTeacherAsync(int id)
    {
        var teacher = await _dbContext.Teachers.Where(t => t.TeacherId == id)
            .Include(t => t.Department)
            .ThenInclude(d => d.TeachersFaculty)
            .FirstOrDefaultAsync();

        if (teacher == null)
        {
            return null;
        }

        return new TeacherDto
        {
            Id = teacher.TeacherId,
            ShortName = teacher.ShortName,
            FullName = teacher.FullName,
            Faculty = new Entity
            {
                Id = teacher.Department.TeachersFaculty.FacultyId,
                ShortName = teacher.Department.TeachersFaculty.ShortName,
                FullName = teacher.Department.TeachersFaculty.FullName,
            },
            Department = new Entity
            {
                Id = teacher.Department.DepartmentId,
                ShortName = teacher.Department.ShortName,
                FullName = teacher.Department.FullName,
            },
        };
    }

    public async Task<AuditoryDto?> GetAuditoryAsync(int id)
    {
        var auditory = await _dbContext.Auditories.Where(b => b.AuditoryId == id)
            .Include(a => a.Building)
            .Include(a => a.AuditoryTypes)
            .FirstOrDefaultAsync();

        if (auditory == null)
        {
            return null;
        }

        return new AuditoryDto
        {
            Id = auditory.AuditoryId,
            Name = auditory.ShortName,
            Floor = auditory.Floor,
            HasPower = auditory.HasPower,
            Building = new MinimalBuilding
            {
                Id = auditory.Building.BuildingId,
                ShortName = auditory.Building.ShortName,
                FullName = auditory.Building.FullName,
            },
            AuditoryTypes = auditory.AuditoryTypes.Select(at => new AuditoryTypeDto
            {
                Id = at.AuditoryTypeId,
                Name = at.Name,
            }).ToList(),
        };
    }

    #endregion

    #region Schedule

    public async Task<List<LessonDto>?> GetLessonsAsync(int id, EntityType entityType, int? startTime = null, int? endTime = null)
    {
        var lessons = await _dbContext.Lessons
            .Include(l => l.Groups)
            .ThenInclude(g => g.Direction)
            .ThenInclude(d => d.GroupsFaculty)
            .Include(l => l.Teachers)
            .ThenInclude(t => t.Department)
            .ThenInclude(d => d.TeachersFaculty)
            .Include(l => l.Auditory)
            .ThenInclude(a => a.AuditoryTypes)
            .Include(l => l.Auditory)
            .ThenInclude(a => a.Building)
            .Include(l => l.Type)
            .ToListAsync();

        var lessonsDto = new List<LessonDto>();

        foreach (var lesson in lessons)
        {
            // TODO: Remove duplicate teachers

            switch (entityType)
            {
                case EntityType.Group:
                    if (lesson.Groups.Any(g => g.GroupId == id))
                    {
                        lessonsDto.Add(lesson.ToLessonDto());
                    }
                    break;

                case EntityType.Teacher:
                    if (lesson.Teachers.Any(t => t.TeacherId == id))
                    {
                        lessonsDto.Add(lesson.ToLessonDto());
                    }
                    break;

                case EntityType.Auditory:
                    if (lesson.Auditory.AuditoryId == id)
                    {
                        lessonsDto.Add(lesson.ToLessonDto());
                    }
                    break;
            }
        }

        lessonsDto.RemoveAll(l => startTime != null && l.StartTime <= startTime);
        lessonsDto.RemoveAll(l => endTime != null && l.EndTime >= endTime);

        lessonsDto.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

        return lessonsDto;
    }

    public async Task<List<LessonDto>?> GetLessonsAsync(string name, EntityType entityType, int? startTime = null, int? endTime = null)
    {
        var id = entityType switch
        {
            EntityType.Group => await _dbContext.Groups.Where(g => g.Name == name).Select(g => g.GroupId).FirstOrDefaultAsync(),
            EntityType.Teacher => await _dbContext.Teachers.Where(t => t.ShortName == name).Select(t => t.TeacherId).FirstOrDefaultAsync(),
            EntityType.Auditory => await _dbContext.Auditories.Where(a => a.ShortName == name).Select(a => a.AuditoryId).FirstOrDefaultAsync(),
            _ => throw new ArgumentException($"Invalid entity type: {entityType}"),
        };

        return await GetLessonsAsync(id, entityType, startTime, endTime);
    }

    #endregion

    #endregion

    #region Private methods

    #region Delete methods

    private async Task DeleteAllGroupsFacultiesFromDb()
    {
        _dbContext.Groups.RemoveRange(_dbContext.Groups);
        _dbContext.Directions.RemoveRange(_dbContext.Directions);
        _dbContext.GroupsFaculty.RemoveRange(_dbContext.GroupsFaculty);
        await _dbContext.SaveChangesAsync();
    }

    private async Task DeleteAllTeachersFacultiesFromDb()
    {
        _dbContext.Departments.RemoveRange(_dbContext.Departments);
        _dbContext.TeachersFaculties.RemoveRange(_dbContext.TeachersFaculties);
        await _dbContext.SaveChangesAsync();
    }

    private async Task DeleteAllBuildingsFromDb()
    {
        _dbContext.AuditoryTypes.RemoveRange(_dbContext.AuditoryTypes);
        _dbContext.Auditories.RemoveRange(_dbContext.Auditories);
        _dbContext.Buildings.RemoveRange(_dbContext.Buildings);
        await _dbContext.SaveChangesAsync();
    }

    private async Task DeleteLessonsFromDbAsync(int id, EntityType entityType)
    {
        IEnumerable<LessonDomain> lessonsToRemove = entityType switch
        {
            EntityType.Group => _dbContext.Lessons.Include(l => l.Type).Include(l => l.Groups).Where(l => l.Groups.Any(g => g.GroupId == id)),
            EntityType.Teacher => _dbContext.Lessons.Include(l => l.Type).Include(l => l.Teachers).Where(l => l.Teachers.Any(t => t.TeacherId == id)),
            EntityType.Auditory => _dbContext.Lessons.Include(l => l.Type).Include(l => l.Auditory).Where(l => l.Auditory.AuditoryId == id),
            _ => Enumerable.Empty<LessonDomain>().AsQueryable()
        };
        foreach (var lesson in lessonsToRemove)
        {
            lesson.Groups.Clear();
            lesson.Teachers.Clear();
            lesson.TypeId = Guid.Empty;

            _dbContext.Lessons.Update(lesson);
        }

        _dbContext.Lessons.RemoveRange(lessonsToRemove);
        await _dbContext.SaveChangesAsync();
    }

    #endregion

    private async Task<List<LessonDomain>> ConvertCistScheduleToLessons(CistSchedule cistSchedule)
    {
        var lessonsDomain = new List<LessonDomain>();
        var auditories = await _dbContext.Auditories.Include(a => a.Building).Include(a => a.AuditoryTypes).ToListAsync();
        var groups = await _dbContext.Groups.Include(g => g.Direction).ThenInclude(d => d.GroupsFaculty).ToListAsync();
        var teachers = await _dbContext.Teachers.Include(g => g.Department).ThenInclude(d => d.TeachersFaculty).ToListAsync();
        var types = await _dbContext.LessonTypes.ToListAsync();

        if (types.Count != 20)
        {
            _dbContext.LessonTypes.RemoveRange(types);
            _dbContext.LessonTypes.AddRange(cistSchedule.Types);

            _dbContext.SaveChanges();
        }

        // Remove groups and teachers that are not in the schedule
        groups.RemoveAll(g => cistSchedule.Groups.All(cg => cg.Id != g.GroupId));
        teachers.RemoveAll(t => cistSchedule.Teachers.All(ct => ct.TeacherId != t.TeacherId));

        foreach (var _event in cistSchedule.Events)
        {
            lessonsDomain.Add(new LessonDomain
            {
                LessonId = _event.SubjectId,
                Brief = cistSchedule.Subjects.FirstOrDefault(subject => subject.Id == _event.SubjectId)?.Brief,
                Title = cistSchedule.Subjects.FirstOrDefault(subject => subject.Id == _event.SubjectId)?.Title,
                TypeId = types.FirstOrDefault(t => t.TypeId == _event.Type).Id,
                Type = types.FirstOrDefault(t => t.TypeId == _event.Type),
                StartTime = _event.StartTime,
                EndTime = _event.EndTime,
                NumberPair = _event.NumberPair,
                Auditory = auditories.Find(a => a.ShortName == _event.Auditory),
                AuditoryId = auditories.Find(a => a.ShortName == _event.Auditory).Id,
                GroupIds = groups.Where(group => _event.GroupIds.Contains(group.GroupId)).ToList().Select(g => g.Id).ToList(),
                TeacherIds = teachers.Where(teacher => _event.TeacherIds.Contains(teacher.TeacherId)).ToList().Select(t => t.Id).ToList(),
                Groups = groups.Where(group => _event.GroupIds.Contains(group.GroupId)).ToList(),
                Teachers = teachers.Where(teacher => _event.TeacherIds.Contains(teacher.TeacherId)).ToList(),
            });
        }
        lessonsDomain.Sort((a, b) => b.StartTime.CompareTo(a.StartTime));

        return lessonsDomain;
    }

    #region Saving methods

    private async Task<List<LessonDto>?> SaveLessonsToDbAsync(int id, CistSchedule cistSchedule, EntityType entityType)
    {
        var lessons = await ConvertCistScheduleToLessons(cistSchedule);

        var fetchLog = await _dbContext.ScheduleFetchLogs.FirstOrDefaultAsync(log =>
            log.Type == entityType && log.EntityId == id);

        if (fetchLog != null)
        {
            fetchLog.LastFetchedAt = DateTime.UtcNow;
            _dbContext.ScheduleFetchLogs.Update(fetchLog);
        }
        else
        {
            fetchLog = new ScheduleFetchLog
            {
                LastFetchedAt = DateTime.UtcNow,
                Type = entityType,
                EntityId = id,
            };

            // Getting the most recent executed job
            var lastJob = JobStorage.Current.GetConnection().GetRecurringJobs().ToList()
                .OrderByDescending(j => j.LastExecution).FirstOrDefault();

            RecurringJob.AddOrUpdate<ScheduleFetch>(
                $"update-{entityType.ToString().ToLower()}-with-id-{id}",
                job => job.Execute(id, cistSchedule, entityType),
                (lastJob != null && lastJob.NextExecution != null) ?
                Cron.Daily(lastJob.NextExecution.Value.Hour, lastJob.NextExecution.Value.Minute + 1) :
                Cron.Daily(DateTime.Now.Hour, DateTime.Now.Minute)
            );

            _dbContext.ScheduleFetchLogs.Add(fetchLog);
        }

        foreach (var lesson in lessons)
        {
            await _dbContext.Lessons.AddAsync(lesson);
        }

        await _dbContext.SaveChangesAsync();

        return lessons.Select(l => l.ToLessonDto()).ToList();
    }

    /// <summary>
    /// Saves groups faculties to the database.
    /// </summary>
    /// <param name="p_groupsFaculties"></param>
    /// <returns></returns>
    private async Task<List<GroupsFacultyDomain>?> SaveGroupsFacultiesToDbAsync(List<CistGroupsFaculty> p_groupsFaculties)
    {
        var groupsFaculties = new List<GroupsFacultyDomain>();

        foreach (var faculty in p_groupsFaculties)
        {
            var facultyDomain = new GroupsFacultyDomain
            {
                FacultyId = faculty.Id,
                ShortName = faculty.ShortName,
                FullName = faculty.FullName,
                Directions = [],
            };

            foreach (var direction in faculty.Directions)
            {
                var directionDomain = new DirectionDomain
                {
                    DirectionId = direction.Id,
                    ShortName = direction.ShortName,
                    FullName = direction.FullName,
                    Groups = [],
                    GroupsFacultyDomainId = facultyDomain.Id,
                };

                if (direction.Groups.Count > 0)
                {
                    foreach (var group in direction.Groups)
                    {
                        directionDomain.Groups.Add(new Group
                        {
                            GroupId = group.GroupId,
                            Name = group.Name,
                        });
                    }
                }
                else
                {
                    foreach (var specialty in direction.Specialties)
                    {
                        foreach (var group in specialty.Groups)
                        {
                            directionDomain.Groups.Add(new Group
                            {
                                GroupId = group.GroupId,
                                Name = group.Name,
                            });
                        }
                    }
                }

                facultyDomain.Directions.Add(directionDomain);
            }

            groupsFaculties.Add(facultyDomain);
        }

        _dbContext.GroupsFaculty.AddRange(groupsFaculties);
        await _dbContext.SaveChangesAsync();

        return groupsFaculties;
    }

    /// <summary>
    /// Saves teachers faculties to the database.
    /// </summary>
    /// <param name="p_teachersFaculties"></param>
    /// <returns></returns>
    private async Task<List<TeachersFacultyDomain>?> SaveTeachersFacultiesToDbAsync(List<CistTeachersFaculty> p_teachersFaculties)
    {
        var teachersFaculties = new List<TeachersFacultyDomain>();

        foreach (var faculty in p_teachersFaculties)
        {
            var facultyDomain = new TeachersFacultyDomain
            {
                FacultyId = faculty.Id,
                ShortName = faculty.ShortName,
                FullName = faculty.FullName,
                Departments = [],
            };

            foreach (var department in faculty.Departments)
            {
                if (department.Teachers.Count > 0)
                {
                    facultyDomain.Departments.Add(department.ToDepartmentDomain(facultyDomain));
                }
                else
                {
                    var subFacultyDomain = new TeachersFacultyDomain
                    {
                        FacultyId = department.Id,
                        ShortName = department.ShortName,
                        FullName = department.FullName,
                        Departments = [],
                    };

                    foreach (var subDepartment in department.Departments)
                    {
                        subFacultyDomain.Departments.Add(new DepartmentDomain
                        {
                            DepartmentId = subDepartment.Id,
                            ShortName = subDepartment.ShortName,
                            FullName = subDepartment.FullName,
                            Teachers = subDepartment.Teachers,
                            TeachersFaculty = subFacultyDomain,
                            TeachersFacultyDomainId = subFacultyDomain.Id,
                        });
                    }
                    teachersFaculties.Add(subFacultyDomain);
                }
            }

            if (facultyDomain.Departments.Count > 0)
            {
                teachersFaculties.Add(facultyDomain);
            }
        }

        _dbContext.TeachersFaculties.AddRange(teachersFaculties);
        await _dbContext.SaveChangesAsync();

        return teachersFaculties;
    }

    /// <summary>
    /// Saves buildings to the database.
    /// </summary>
    /// <param name="p_buildings"></param>
    /// <returns></returns>
    private async Task<List<BuildingDomain>?> SaveBuildingsToDbAsync(List<CistBuilding> p_buildings)
    {
        var buildings = p_buildings.Select(b => new BuildingDomain
        {
            BuildingId = b.Id,
            ShortName = b.ShortName,
            FullName = b.FullName,
            Auditories = b.Auditories.Select(cistAuditory =>
            new AuditoryDomain
            {
                AuditoryId = cistAuditory.Id,
                ShortName = cistAuditory.ShortName,
                Floor = cistAuditory.Floor,
                HasPower = cistAuditory.HasPower,
                AuditoryTypes = cistAuditory.AuditoryTypes.Select(at =>
                new AuditoryTypeDomain
                {
                    AuditoryTypeId = at.Id,
                    Name = at.ShortName,
                }
                ).ToList(),
            }).ToList()
        }).ToList();

        _dbContext.Buildings.AddRange(buildings);
        await _dbContext.SaveChangesAsync();

        return buildings;
    }

    #endregion

    #endregion
}
