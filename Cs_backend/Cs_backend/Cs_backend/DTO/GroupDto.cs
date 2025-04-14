using Cs_backend.Interfaces;
using Cs_backend.Models;

namespace Cs_backend.DTO;

public class GroupDto : ICastable<Group>
{
    public int GroupId { get; set; }
    
    public required string Name { get; set; }

    public Group Cast()
    {
        return new Group()
        {
            Id = GroupId,
            Name = Name
        };
    }
}