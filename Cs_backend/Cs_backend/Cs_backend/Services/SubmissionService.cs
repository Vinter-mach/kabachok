using Cs_backend.DTO;
using Cs_backend.Infrastructure;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Cs_backend.Services.BaseServices;
using System.Threading.Tasks;
using Task = Cs_backend.Models.Task;

namespace Cs_backend.Services;

public class SubmissionService(SubmissionRepository submissionRepository)
{
    public async System.Threading.Tasks.Task AddOrUpdateSubmission(SubmittedTask task)
    {
        await submissionRepository.AddOrUpdateAsync(task);
    }

    public async Task<List<List<SubmissionDto>>> GetSubmissionsByTaskId(int taskId)
    {
        var submissions = await GetterByFunc.GetByFunc<SubmittedTask, SubmissionDto>(submissionRepository,
            x => x.TaskId == taskId);

        var envVariables = CloudFileGetter.GetEnvironmentVariables();

        var tasks = submissions.Select(x => x.Cast())
            .Select(submission => ProcessSubmissionAsync(submission, envVariables)).ToList();

        var results = await System.Threading.Tasks.Task.WhenAll(tasks);
        return results.ToList();
    }

    private async Task<List<SubmissionDto>> ProcessSubmissionAsync(SubmittedTask submission,
        (string accessKey, string secretKey, string backet, string endpoint) envVariables)
    {
        var links = await CloudFileGetter.GetTemporaryLinks(
            envVariables.endpoint,
            envVariables.backet,
            envVariables.accessKey,
            envVariables.secretKey,
            submission.HomeworkPrefix,
            DateTime.UtcNow.AddHours(1)
        );

        return links.Select(link =>
        {
            var dto = submission.Cast();
            dto.homeworkFile = link;
            return dto;
        }).ToList();
    }
}