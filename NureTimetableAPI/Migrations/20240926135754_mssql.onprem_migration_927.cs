using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NureTimetableAPI.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_927 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoryTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditoryTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupsFaculty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsFaculty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdBase = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleFetchLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastFetchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleFetchLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeachersFaculties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachersFaculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DirectionId = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupsFacultyDomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Directions_GroupsFaculty_GroupsFacultyDomainId",
                        column: x => x.GroupsFacultyDomainId,
                        principalTable: "GroupsFaculty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditoryId = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    HasPower = table.Column<bool>(type: "bit", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleFetchLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditories_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auditories_ScheduleFetchLogs_ScheduleFetchLogId",
                        column: x => x.ScheduleFetchLogId,
                        principalTable: "ScheduleFetchLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeachersFacultyDomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_TeachersFaculties_TeachersFacultyDomainId",
                        column: x => x.TeachersFacultyDomainId,
                        principalTable: "TeachersFaculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleFetchLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_ScheduleFetchLogs_ScheduleFetchLogId",
                        column: x => x.ScheduleFetchLogId,
                        principalTable: "ScheduleFetchLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditoryDomainAuditoryTypeDomain",
                columns: table => new
                {
                    AuditoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditoryTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoryDomainAuditoryTypeDomain", x => new { x.AuditoriesId, x.AuditoryTypesId });
                    table.ForeignKey(
                        name: "FK_AuditoryDomainAuditoryTypeDomain_Auditories_AuditoriesId",
                        column: x => x.AuditoriesId,
                        principalTable: "Auditories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditoryDomainAuditoryTypeDomain_AuditoryTypes_AuditoryTypesId",
                        column: x => x.AuditoryTypesId,
                        principalTable: "AuditoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleLogIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    Brief = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberPair = table.Column<int>(type: "int", nullable: false),
                    TeacherIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Auditories_AuditoryId",
                        column: x => x.AuditoryId,
                        principalTable: "Auditories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_LessonTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "LessonTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleFetchLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_ScheduleFetchLogs_ScheduleFetchLogId",
                        column: x => x.ScheduleFetchLogId,
                        principalTable: "ScheduleFetchLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupLessons",
                columns: table => new
                {
                    LessonDomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupLessons", x => new { x.LessonDomainId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupLessons_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupLessons_Lessons_LessonDomainId",
                        column: x => x.LessonDomainId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherLessons",
                columns: table => new
                {
                    LessonDomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherLessons", x => new { x.LessonDomainId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_TeacherLessons_Lessons_LessonDomainId",
                        column: x => x.LessonDomainId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherLessons_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditories_BuildingId",
                table: "Auditories",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditories_ScheduleFetchLogId",
                table: "Auditories",
                column: "ScheduleFetchLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoryDomainAuditoryTypeDomain_AuditoryTypesId",
                table: "AuditoryDomainAuditoryTypeDomain",
                column: "AuditoryTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TeachersFacultyDomainId",
                table: "Departments",
                column: "TeachersFacultyDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Directions_GroupsFacultyDomainId",
                table: "Directions",
                column: "GroupsFacultyDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupLessons_GroupId",
                table: "GroupLessons",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DirectionId",
                table: "Groups",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ScheduleFetchLogId",
                table: "Groups",
                column: "ScheduleFetchLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_AuditoryId",
                table: "Lessons",
                column: "AuditoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TypeId",
                table: "Lessons",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLessons_TeacherId",
                table: "TeacherLessons",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ScheduleFetchLogId",
                table: "Teachers",
                column: "ScheduleFetchLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoryDomainAuditoryTypeDomain");

            migrationBuilder.DropTable(
                name: "GroupLessons");

            migrationBuilder.DropTable(
                name: "TeacherLessons");

            migrationBuilder.DropTable(
                name: "AuditoryTypes");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Directions");

            migrationBuilder.DropTable(
                name: "Auditories");

            migrationBuilder.DropTable(
                name: "LessonTypes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "GroupsFaculty");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "ScheduleFetchLogs");

            migrationBuilder.DropTable(
                name: "TeachersFaculties");
        }
    }
}
