using Cs_backend.Database;
using Microsoft.EntityFrameworkCore;

namespace Cs_backend.Services;

public class CoursesGetter(ApplicationContext context)
{
    public async Task<List<CourseToJson>> GetCourses()
    {
        var courses = await context.Courses.AsNoTracking().ToListAsync();
        return courses.Select(course => new CourseToJson
        {
            CourseId = course.Id,
            CourseName = course.Name
        }).ToList();
    }
}

public class CourseToJson
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
}