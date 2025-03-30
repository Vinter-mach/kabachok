using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class SubmittedTask(
    Guid studentId,
    Student student,
    Guid taskId,
    Task task,
    Guid statusId,
    TaskStatus status,
    string homeworkLink,
    DateOnly submittedDate,
    int grade,
    string comment)
    : BaseModel
{
    public Guid StudentId { get; set; } = studentId;
    public Student Student { get; set; } = student;
    public Guid TaskId { get; set; } = taskId;
    public Task Task { get; set; } = task;
    public Guid StatusId { get; set; } = statusId;
    public TaskStatus Status { get; set; } = status;
    public string HomeworkLink { get; set; } = homeworkLink;
    public DateOnly SubmittedDate { get; set; } = submittedDate;
    public int Grade { get; set; } = grade;
    public string Comment { get; set; } = comment;
}


public class SubmittedTasksConfiguration : IEntityTypeConfiguration<SubmittedTask>
{
    private const int MaxCommentLength = 1000;
    public void Configure(EntityTypeBuilder<SubmittedTask> builder)
    {
        builder.Property(x => x.HomeworkLink)
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(x => x.Comment)
            .HasMaxLength(MaxCommentLength);
        builder.Property(x => x.SubmittedDate)
            .IsRequired();
    }
}