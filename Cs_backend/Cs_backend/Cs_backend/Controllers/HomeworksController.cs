using Cs_backend.DTO;
using Cs_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[Route("courses/{courseId:int}/{taskId:int}/submissions/{submissionId:int}")]
public class HomeworkController(HomeworkService homeworkService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetSubmissionBySubmissionId(int courseId, int taskId, int submissionId)
    {
        var submission = await homeworkService.GetSubmissionBySubmissionId(submissionId);
        return new JsonResult(submission);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddOrUpdateSubmission(int courseId, int taskId, int submissionId, [FromBody] SubmissionDto dto)
    {
        var submittedTask = dto.Cast();
        submittedTask.Id = submissionId;
        await homeworkService.UpdateSubmission(submittedTask);
        return Ok(new { message = "Дз успешно проверено" });
    }
}