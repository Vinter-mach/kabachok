using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Course(string name, BigInteger passwordHash) : BaseModel
{
    public string Name { get; set; } = name;
    public BigInteger PasswordHash { get; set; } = passwordHash;
    public ICollection<Student> Students { get; set; }
    public ICollection<Task> Tasks { get; set; }
}

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.Name);
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}