using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

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