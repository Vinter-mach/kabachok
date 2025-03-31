using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Course: BaseModel
{
    public string Name { get; set; }
    public BigInteger PasswordHash { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<Task> Tasks { get; set; }
    public ICollection<TeacherCourse> TeacherCourses { get; set; }
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