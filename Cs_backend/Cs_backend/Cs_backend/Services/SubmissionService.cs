using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Cs_backend.Services.BaseServices;
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
        return await GetterByFunc.GetByFunc<SubmittedTask, SubmissionDto>(submissionRepository,
            x => x.TaskId == taskId);
    }
}