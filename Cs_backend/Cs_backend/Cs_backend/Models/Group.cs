using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Group(string name) : BaseModel
{
    public string Name { get; set; } = name;
    public ICollection<Student> Students { get; set; }
}


public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(50);
    }
}