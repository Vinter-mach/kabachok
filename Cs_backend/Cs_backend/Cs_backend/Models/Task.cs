using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Task(
    string topic,
    string taskLink,
    DateOnly deadline,
    string teacher,
    bool isGrave,
    Guid courseId,
    Course course)
    : BaseModel
{
    public string Topic { get; set; } = topic;
    public string TaskLink { get; set; } = taskLink;
    public DateOnly Deadline { get; set; } = deadline;
    public string Teacher { get; set; } = teacher;
    public bool IsGrave { get; set; } = isGrave;
    public Guid CourseId { get; set; } = courseId;
    public Course Course { get; set; } = course;
    public ICollection<SubmittedTask> SubmittedTasks { get; set; }
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