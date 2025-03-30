using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;
using Task = Cs_backend.Models.Task;
using TaskStatus = Cs_backend.Models.TaskStatus;


namespace Cs_backend.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<TaskStatus> Statuses { get; set; }
    public DbSet<SubmittedTask> SubmittedTasks { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    {
        throw new NotImplementedException();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}