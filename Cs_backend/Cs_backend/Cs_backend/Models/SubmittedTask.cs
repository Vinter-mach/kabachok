using Cs_backend.DTO;
using Cs_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class SubmittedTask
    : BaseModel, ICastable<SubmissionDto>
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
    public int StatusId { get; set; }
    public TaskStatus Status { get; set; }
    public string HomeworkPrefix { get; set; }
    public DateTime SubmittedDate { get; set; }
    public int Grade { get; set; }
    public string Comment { get; set; }

    public SubmissionDto Cast()
    {
        return new SubmissionDto()
        {
            SubmissionId = Id,
            TaskId = TaskId,
            StudentId = StudentId,
            StatusId = StatusId,
            homeworkFile = HomeworkPrefix,
            Date = SubmittedDate,
            Grade = Grade,
            Comment = Comment
        };
    }
}

public class SubmittedTasksConfiguration : IEntityTypeConfiguration<SubmittedTask>
{
    private const int MaxCommentLength = 1000;

    public void Configure(EntityTypeBuilder<SubmittedTask> builder)
    {
        builder.Property(x => x.HomeworkPrefix)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(x => x.Comment)
            .HasMaxLength(MaxCommentLength);
        builder.Property(x => x.SubmittedDate)
            .IsRequired();
    }
}