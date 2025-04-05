using Cs_backend.Models;
using Cs_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[ApiController]
[Route("courses")]
public class CoursesController(CoursesGetter coursesGetter) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await coursesGetter.GetCourses();
        return new JsonResult(courses);
    }
}