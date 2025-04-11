using Cs_backend.Models;
using Cs_backend.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Services;

public class CourseService(IRepository<Course> courseRepository)
{
    public async Task AddOrUpdateCourse(Course course)
    {
        await courseRepository.AddOrUpdateAsync(course);
    }

    public async Task<List<CourseToJson>> GetCourses()
    {
        var courses = await courseRepository.GetAllAsync();
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