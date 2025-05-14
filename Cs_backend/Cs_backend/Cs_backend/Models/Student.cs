using Cs_backend.DTO;
using Cs_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Student
    : BaseModel, ICastable<StudentDto>
{
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public string Name { get; set; }
    public long TelegramId { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public ICollection<SubmittedTask> SubmittedTasks { get; set; }
    public StudentDto Cast()
    {
        return new StudentDto
        {
            StudentId = Id,
            Name = Name,
            GroupId = GroupId,
            TgId = TelegramId,
            CourseId = CourseId
        };
    }
}

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
    }
}