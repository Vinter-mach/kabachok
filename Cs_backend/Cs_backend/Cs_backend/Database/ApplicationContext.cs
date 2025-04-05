using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Task = Cs_backend.Models.Task;
using TaskStatus = Cs_backend.Models.TaskStatus;


namespace Cs_backend.Database;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<TaskStatus> Statuses { get; set; }
    public DbSet<SubmittedTask> SubmittedTasks { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeacherCourse> TeacherCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Database=project");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }
        }
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.Entity<Student>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Students);
        modelBuilder.Entity<Student>()
            .HasOne(x => x.Course)
            .WithMany(x => x.Students);
        modelBuilder.Entity<Student>()
            .HasMany(x => x.SubmittedTasks)
            .WithOne(x => x.Student);
        modelBuilder.Entity<Course>()
            .HasMany(x => x.Tasks)
            .WithOne(x => x.Course);
        modelBuilder.Entity<Task>()
            .HasMany(x => x.SubmittedTasks)
            .WithOne(x => x.Task);
        modelBuilder.Entity<SubmittedTask>()
            .HasOne(x => x.Status)
            .WithMany(x => x.SubmittedTasks);
        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherCourses);
        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Course)
            .WithMany(c => c.TeacherCourses);
    }
    
    private static string ToSnakeCase(string input)
    {
        return string.Concat(input.Select((c, i) => 
                i > 0 && char.IsUpper(c) ? "_" + c.ToString() : c.ToString()))
            .ToLower();
    }
}