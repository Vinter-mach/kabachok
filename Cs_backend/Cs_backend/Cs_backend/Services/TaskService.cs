using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
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
        var tasks = await taskRepository.GetTasksByCourseId(courseId);
        return tasks.Select(task => new TaskDto()
        {
            TaskId = task.Id,
            Name = task.Topic,
            TaskLink = task.TaskLink,
            Deadline = task.Deadline,
            Teacher = task.Teacher,
            IsGraves = task.IsGrave,
        }).ToList();
    }
}
