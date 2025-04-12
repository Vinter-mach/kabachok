using Task = Cs_backend.Models.Task;

namespace Cs_backend.DTO;

public class TaskDto
{
    public int TaskId { get; set; }
    public required string Name { get; set; }
    public required string TaskLink { get; set; }
    public DateOnly Deadline { get; set; }
    public required string Teacher { get; set; }
    public bool IsGraves { get; set; }

    public Task ToTask()
    {
        return new Task
        {
            Topic = Name,
            Id = TaskId,
            TaskLink = TaskLink,
            Deadline = Deadline,
            Teacher = Teacher,
            IsGrave = IsGraves,
        };
    }
}