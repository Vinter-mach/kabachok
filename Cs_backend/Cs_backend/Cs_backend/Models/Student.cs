using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Student
    : BaseModel
{
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public string Name { get; set; }
    public int TelegramId { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public ICollection<SubmittedTask> SubmittedTasks { get; set; }
}

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}