using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Cs_backend.Services.BaseServices;
using Task = Cs_backend.Models.Task;

namespace Cs_backend.Services;

public class TaskService(TaskRepository taskRepository)
{
    public async System.Threading.Tasks.Task AddOrUpdateTask(Task task)
    {
        await taskRepository.AddOrUpdateAsync(task);
    }

    public async Task<List<TaskDto>> GetTasksByCourseId(int courseId)
    {
        return await GetterByFunc.GetByFunc<Task, TaskDto>(taskRepository, x => x.CourseId == courseId);
    }
}
