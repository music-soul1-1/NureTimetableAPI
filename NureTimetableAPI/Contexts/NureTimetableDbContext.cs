using Microsoft.EntityFrameworkCore;
using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Domain;

namespace NureTimetableAPI.Contexts;

public class NureTimetableDbContext(DbContextOptions<NureTimetableDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GroupsFacultyDomain>()
            .HasKey(f => f.Id);

        modelBuilder.Entity<TeachersFacultyDomain>()
            .HasKey(f => f.Id);

        modelBuilder.Entity<BuildingDomain>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<DirectionDomain>()
            .HasKey(d => d.Id);

        modelBuilder.Entity<DirectionDomain>()
            .HasMany(d => d.Groups)
            .WithOne(g => g.Direction)
            .HasForeignKey(g => g.DirectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DirectionDomain>()
            .HasOne(d => d.GroupsFaculty)
            .WithMany(f => f.Directions)
            .HasForeignKey(d => d.GroupsFacultyDomainId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DepartmentDomain>()
            .HasKey(d => d.Id);

        modelBuilder.Entity<DepartmentDomain>()
            .HasMany(d => d.Teachers)
            .WithOne(t => t.Department)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DepartmentDomain>()
            .HasOne(d => d.TeachersFaculty)
            .WithMany(tf => tf.Departments)
            .HasForeignKey(d => d.TeachersFacultyDomainId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AuditoryDomain>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<AuditoryDomain>()
            .HasMany(a => a.AuditoryTypes)
            .WithMany(at => at.Auditories);

        modelBuilder.Entity<AuditoryDomain>()
            .HasOne(a => a.Building)
            .WithMany(b => b.Auditories)
            .HasForeignKey(a => a.BuildingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Specialty>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Group>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<Teacher>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<AuditoryTypeDomain>()
            .HasKey(at => at.Id);

        modelBuilder.Entity<AuditoryTypeDomain>()
            .HasMany(at => at.Auditories)
            .WithMany(a => a.AuditoryTypes);
    }

    public DbSet<GroupsFacultyDomain> GroupsFaculty { get; set; }

    public DbSet<TeachersFacultyDomain> TeachersFaculties { get; set; }

    public DbSet<BuildingDomain> Buildings { get; set; }

    public DbSet<DirectionDomain> Directions { get; set; }

    public DbSet<DepartmentDomain> Departments { get; set; }

    public DbSet<AuditoryDomain> Auditories { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<AuditoryTypeDomain> AuditoryTypes { get; set; }
}
