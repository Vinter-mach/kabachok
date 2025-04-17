using System.Numerics;
using Cs_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Teacher : BaseModel
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<TeacherCourse> TeacherCourses { get; set; }
    public ICollection<Task> Tasks { get; set; }
}

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.Property(t => t.Login)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(t => t.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(256);
    }
}
