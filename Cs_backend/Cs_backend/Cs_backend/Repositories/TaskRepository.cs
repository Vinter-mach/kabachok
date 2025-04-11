using Cs_backend.Database;
using Microsoft.EntityFrameworkCore;
using Task = Cs_backend.Models.Task;

namespace Cs_backend.Repositories;

public class TaskRepository(ApplicationContext applicationContext)
    : BaseRepository<Task>(applicationContext, applicationContext.Tasks)
{
    public async Task<List<Task>> GetTasksByCourseId(int courseId)
    {
        return await applicationContext.Tasks.AsNoTracking().Where(t => t.CourseId == courseId).ToListAsync();
    }
}