using Cs_backend.Interfaces;
using Cs_backend.Models;

namespace Cs_backend.DTO;

public class CourseDto : ICastable<Course>
{
    public int CourseId { get; set; }
    
    public required string Name { get; set; }

    public Course Cast()
    {
        return new Course()
        {
            Id = CourseId,
            Name = Name
        };
    }
}