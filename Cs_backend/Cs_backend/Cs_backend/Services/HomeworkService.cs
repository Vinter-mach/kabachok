using Amazon.S3;
using Amazon.S3.Model;
using Cs_backend.DTO;
using Cs_backend.Infrastructure;
using Cs_backend.Infrastructure;
using Cs_backend.Models;
using Cs_backend.Repositories;

namespace Cs_backend.Services;

public class HomeworkService(HomeworkRepository homeworkRepository)
{
    public async System.Threading.Tasks.Task UpdateSubmission(SubmittedTask task)
    {
        await homeworkRepository.UpdateAsync(task);
    }

    public async Task<List<SubmissionDto>> GetSubmissionBySubmissionId(int submissionId)
    {
        var submission = await homeworkRepository.GetByIdAsync(submissionId);
        if (submission is null)
        {
            return new List<SubmissionDto>();
        }

        var envVariables = CloudFileGetter.GetEnvironmentVariables();
        var links = await CloudFileGetter.GetTemporaryLinks(
            envVariables.endpoint,
            envVariables.backet,
            envVariables.accessKey,
            envVariables.secretKey,
            submission.HomeworkPrefix,
            DateTime.UtcNow.AddHours(1));
        return links.Select(link =>
        {
            var sub = submission.Cast();
            sub.homeworkFile = link;
            return sub;
        }).ToList();
    }
}