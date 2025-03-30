namespace Cs_backend.Services;

public class TasksGetter
{
    public ICollection<(Guid TaskId, string TaskName, int Number)> GetTasks(Guid courseId)
    {
        throw new NotImplementedException();
    }
}