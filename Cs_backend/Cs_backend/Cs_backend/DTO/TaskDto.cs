using Cs_backend.Interfaces;
using Task = Cs_backend.Models.Task;

namespace Cs_backend.DTO;

public class TaskDto : ICastable<Task>
{
    public int TaskId { get; set; }
    public required string Name { get; set; }
    public required string TaskLink { get; set; }
    public DateTime Deadline { get; set; }
    public int TeacherId { get; set; }
    public bool IsGraves { get; set; }

    public Task Cast()
    {
        return new Task
        {
            Topic = Name,
            Id = TaskId,
            TaskLink = TaskLink,
            Deadline = Deadline,
            TeacherId = TeacherId,
            IsGrave = IsGraves,
        };
    }
}