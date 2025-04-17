using Cs_backend.DTO;
using Cs_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Task
    : BaseModel, ICastable<TaskDto>
{
    public string Topic { get; set; }
    public string TaskLink { get; set; }
    public DateOnly Deadline { get; set; }
    public string Teacher { get; set; }
    public bool IsGrave { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public ICollection<SubmittedTask> SubmittedTasks { get; set; }

    public TaskDto Cast()
    {
        return new TaskDto
        {
            TaskId = Id,
            Name = Topic,
            TaskLink = TaskLink,
            Deadline = Deadline,
            Teacher = Teacher,
            IsGraves = IsGrave,
        };
    }
}

public class TasksConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.Property(t => t.Topic)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.TaskLink)
            .HasMaxLength(100);
        builder.Property(t => t.Teacher)
            .IsRequired()
            .HasMaxLength(100);
    }
}