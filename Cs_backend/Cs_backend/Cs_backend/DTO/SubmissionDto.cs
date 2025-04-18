using Cs_backend.Interfaces;
using Cs_backend.Models;

namespace Cs_backend.DTO;

public class SubmissionDto : ICastable<SubmittedTask>
{
    public int SubmissionId { get; set; }
    public int StudentId { get; set; }
    public int TaskId { get; set; }
    public int StatusId { get; set; }
    public string homeworkFile { get; set; }
    public DateOnly Date { get; set; }
    public int Grade { get; set; }
    public string Comment { get; set; }

    public SubmittedTask Cast()
    {
        return new SubmittedTask()
        {
            Id = SubmissionId,
            StudentId = StudentId,
            TaskId = TaskId,
            StatusId = StatusId,
            HomeworkPrefix = homeworkFile,
            SubmittedDate = Date,
            Grade = Grade,
            Comment = Comment
        };
    }
}