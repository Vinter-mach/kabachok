using Cs_backend.DTO;
using Cs_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[Authorize]
[ApiController]
[Route("teachers")]
public class TeachersController (TeachersService teachersService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetTeachersByCourse(int courseId)
    {
        var courses = await teachersService.GetTeachersByCourseId(courseId);
        return new JsonResult(courses);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddOrUpdateTask([FromBody] TeacherDto dto)
    {
        var teacher = dto.Cast();
        await teachersService.AddOrUpdateTeacher(teacher);
        return Ok(new { message = "Таск добавлен или обновлён" });
    }
}