using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Student(Guid groupId, string name, Guid telegramId, Guid courseId, Group group, Course course)
    : BaseModel
{
    public Guid GroupId { get; set; } = groupId;
    public Group Group { get; set; } = group;
    public string Name { get; set; } = name;
    public Guid TelegramId { get; set; } = telegramId;
    public Guid CourseId { get; set; } = courseId;
    public Course Course { get; set; } = course;
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