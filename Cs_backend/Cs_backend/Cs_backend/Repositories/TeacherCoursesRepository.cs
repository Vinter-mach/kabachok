using System.Linq.Expressions;
using Cs_backend.Models;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Repositories;

public class TeacherCoursesRepository
{
    public Task<List<TeacherCourse>> GetByFuncAsync(Expression<Func<TeacherCourse, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<List<TeacherCourse>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddOrUpdateAsync(TeacherCourse entity)
    {
        throw new NotImplementedException();
    }

    public void DeleteAsync(TeacherCourse entity)
    {
        throw new NotImplementedException();
    }
}