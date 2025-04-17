using Cs_backend.DTO;
using Cs_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cs_backend.Models;

public class Group : BaseModel, ICastable<GroupDto>
{
    public string Name { get; set; }
    public ICollection<Student> Students { get; set; }

    public GroupDto Cast()
    {
        return new GroupDto
        {
            GroupId = Id,
            Name = Name
        };
    }
}

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(256);
    }
}