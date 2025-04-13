using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Task = Cs_backend.Models.Task;

namespace Cs_backend.Services;

public class SubmissionService(SubmissionRepository submissionRepository)
{
    public async System.Threading.Tasks.Task AddOrUpdateSubmission(SubmittedTask task)
    {
        await submissionRepository.AddOrUpdateAsync(task);
    }

    public async Task<List<SubmissionDto>> GetSubmissionsByTaskId(int taskId)
    {
        var tasks = await submissionRepository.GetSubmissionsByTaskId(taskId);
        return tasks.Select(task => new SubmissionDto()
        {
            SubmissionId = task.Id,
            TaskId = task.TaskId,
            StudentId = task.StudentId,
            StatusId = task.StatusId,
            homeworkFile = task.HomeworkLink,
            Date = task.SubmittedDate,
            Grade = task.Grade,
            Comment = task.Comment
        }).ToList();
    }
}