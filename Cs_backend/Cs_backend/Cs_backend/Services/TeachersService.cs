using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Cs_backend.Services.BaseServices;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Services;

public class TeachersService(TeachersRepository teachersRepository)
{
    public async Task<List<TeacherDto>> GetTeachersByCourseId(int courseId)
    {
        return await GetterByFunc.GetByFunc<Teacher, TeacherDto>(teachersRepository,
            x => x.TeacherCourses.Select(y => y.CourseId).Contains(courseId));
    }

    public async Task AddOrUpdateTeacher(Teacher teacher)
    {
        await teachersRepository.AddOrUpdateAsync(teacher);
    }

    public async Task InviteTeacherToCourse(int teacherId, int courseId)
    {
        
    }

}