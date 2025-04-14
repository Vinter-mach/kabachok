using System.Numerics;
using Cs_backend.DTO;
using Cs_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Course: BaseModel, ICastable<CourseDto>
{
    public string Name { get; set; }
    public BigInteger PasswordHash { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<Task> Tasks { get; set; }
    public ICollection<TeacherCourse> TeacherCourses { get; set; }
    public CourseDto Cast()
    {
        return new CourseDto()
        {
            CourseId = Id,
            Name = Name
        };
    }
}

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}