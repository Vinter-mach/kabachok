using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class TaskStatus : BaseModel
{
    public string Name { get; set; }
    public ICollection<SubmittedTask> SubmittedTasks { get; set; }
}

public class TaskStatusConfiguration : IEntityTypeConfiguration<TaskStatus>
{
    public void Configure(EntityTypeBuilder<TaskStatus> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(256);
    }
}