using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class SubmittedTask
    : BaseModel
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
    public int StatusId { get; set; } 
    public TaskStatus Status { get; set; }
    public string HomeworkLink { get; set; }
    public DateOnly SubmittedDate { get; set; } 
    public int Grade { get; set; }
    public string Comment { get; set; }
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