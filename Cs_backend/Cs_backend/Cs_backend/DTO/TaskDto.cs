using Task = Cs_backend.Models.Task;

namespace Cs_backend.DTO;

public class TaskDto
{
    public int TaskId { get; set; }
    public required string Name { get; set; }
    public int Number { get; set; }

    public Task ToTask()
    {
        return new Task
        {
            Topic = Name,
            Id = TaskId,
        };
        // TODO - как будто тут сильные различия между полями Task и тем, что есть в API
    }
}