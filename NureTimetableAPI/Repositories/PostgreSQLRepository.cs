using Microsoft.EntityFrameworkCore;
using NureTimetableAPI.Contexts;
using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Models.Domain;
using NureTimetableAPI.Models.Dto;

namespace NureTimetableAPI.Repositories;

public class PostgreSQLRepository(NureTimetableDbContext dbContext) : IPostgreSQLRepository
{
    private readonly NureTimetableDbContext _dbContext = dbContext;

    #region Fetching jobs helpers methods

    private void DeleteAllGroupsFacultiesFromDb()
    {
        _dbContext.Groups.RemoveRange(_dbContext.Groups);
        _dbContext.Directions.RemoveRange(_dbContext.Directions);
        _dbContext.GroupsFaculty.RemoveRange(_dbContext.GroupsFaculty);
        _dbContext.SaveChanges();
    }

    private void DeleteAllTeachersFacultiesFromDb()
    {
        _dbContext.Departments.RemoveRange(_dbContext.Departments);
        _dbContext.TeachersFaculties.RemoveRange(_dbContext.TeachersFaculties);
        _dbContext.SaveChanges();
    }

    private void DeleteAllBuildingsFromDb()
    {
        _dbContext.AuditoryTypes.RemoveRange(_dbContext.AuditoryTypes);
        _dbContext.Auditories.RemoveRange(_dbContext.Auditories);
        _dbContext.Buildings.RemoveRange(_dbContext.Buildings);
        _dbContext.SaveChanges();
    }

    public async Task ClearAndSaveGroupsFacultiesAsync(List<CistGroupsFaculty> p_groupsFaculties)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            DeleteAllGroupsFacultiesFromDb();
            await SaveGroupsFacultiesToDbAsync(p_groupsFaculties);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task ClearAndSaveTeachersFacultiesAsync(List<CistTeachersFaculty> p_teachersFaculties)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            DeleteAllTeachersFacultiesFromDb();

            await SaveTeachersFacultiesToDbAsync(p_teachersFaculties);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task ClearAndSaveBuildingsAsync(List<CistBuilding> p_buildings)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            DeleteAllBuildingsFromDb();
            await SaveBuildingsToDbAsync(p_buildings);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<GroupsFacultyDomain>?> SaveGroupsFacultiesToDbAsync(List<CistGroupsFaculty> p_groupsFaculties)
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

    public async Task<List<TeachersFacultyDomain>?> SaveTeachersFacultiesToDbAsync(List<CistTeachersFaculty> p_teachersFaculties)
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
                    facultyDomain.Departments.Add(department.ToDepartmentDomain());
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

    public async Task<List<BuildingDomain>?> SaveBuildingsToDbAsync(List<CistBuilding> p_buildings)
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

    public Task<CistSchedule> GetCistScheduleAsync(int groupId, int? startTime = null, int? endTime = null)
    {
        throw new NotImplementedException();
    }

    public Task<CistSchedule> GetCistScheduleAsync(string groupName, int? startTime = null, int? endTime = null)
    {
        throw new NotImplementedException();
    }

    public Task<List<LessonDto>> GetLessonsAsync(int groupId, int? startTime = null, int? endTime = null)
    {
        throw new NotImplementedException();
    }

    public Task<List<LessonDto>> GetLessonsAsync(string groupName, int? startTime = null, int? endTime = null)
    {
        throw new NotImplementedException();
    }

    #endregion
}
