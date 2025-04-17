using Cs_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[Authorize]
[ApiController]
[Route("courses/{courseId:int}/{taskId:int}/submissions")]
public class SubmissionController(SubmissionService taskService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetTasksByCourseId(int courseId, int taskId)
    {
        var courses = await taskService.GetSubmissionsByTaskId(taskId);
        return new JsonResult(courses);
    }
}