using Cs_backend.DTO;
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

    public async Task<List<CourseDto>> GetCourses()
    {
        var courses = await courseRepository.GetAllAsync();
        return courses.Select(course => new CourseDto()
        {
            CourseId = course.Id,
            Name = course.Name
        }).ToList();
    }
}