using Cs_backend.Models;

namespace Cs_backend.DTO;

public class GroupDto
{
    public int GroupId { get; set; }
    
    public required string Name { get; set; }

    public Group ToGroup()
    {
        return new Group()
        {
            Id = GroupId,
            Name = Name
        };
    }
}