using System.Data;
using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Xunit.Sdk;

namespace Cs_backend.Services;

public class HomeworkService(HomeworkRepository homeworkRepository)
{
    public async System.Threading.Tasks.Task AddOrUpdateSubmission(SubmittedTask task)
    {
        await homeworkRepository.UpdateAsync(task);
    }

    public async Task<SubmissionDto> GetSubmissionBySubmissionId(int submissionId)
    {
        var submission = await homeworkRepository.GetHomeworkById(submissionId);

        if (submission is null)
            return new SubmissionDto() { Comment =  "решения нет" };
        
        return new SubmissionDto()
        {
            SubmissionId = submission.Id,
            TaskId = submission.TaskId,
            StudentId = submission.StudentId,
            StatusId = submission.StatusId,
            homeworkFile = submission.HomeworkLink,
            Date = submission.SubmittedDate,
            Grade = submission.Grade,
            Comment = submission.Comment
        };
    }
}