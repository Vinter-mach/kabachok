using Cs_backend.Models;

namespace Cs_backend.DTO;

public class CourseDto
{
    public int CourseId { get; set; }
    
    public required string Name { get; set; }

    public Course ToCourse()
    {
        return new Course()
        {
            Id = CourseId,
            Name = Name
        };
    }
}