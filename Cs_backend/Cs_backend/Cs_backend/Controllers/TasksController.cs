using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[ApiController]
[Route("courses/{courseId:int}")]
public class TasksController(TaskService taskService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetTasksByCourseId(int courseId)
    {
        var courses = await taskService.GetTasksByCourseId(courseId);
        return new JsonResult(courses);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddOrUpdateTask(int courseId, [FromBody] TaskDto dto)
    {
        var task = dto.ToTask();
        task.CourseId = courseId;
        await taskService.AddOrUpdateTask(task);
        return Ok(new { message = "Таск добавлен или обновлён" });
    }
}