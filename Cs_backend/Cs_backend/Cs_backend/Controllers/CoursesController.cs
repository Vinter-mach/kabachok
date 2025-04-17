using Cs_backend.DTO;
using Cs_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[Authorize]
[ApiController]
[Route("courses")]
public class CoursesController(CourseService coursesService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await coursesService.GetCourses();
        return new JsonResult(courses);
    }

    [HttpPost] 
    public async Task<IActionResult> AddOrUpdateCourse([FromBody] CourseDto dto)
    {
        await coursesService.AddOrUpdateCourse(dto.Cast());

        return Ok(new { message = "Курс добавлен или обновлён" });
    }
}