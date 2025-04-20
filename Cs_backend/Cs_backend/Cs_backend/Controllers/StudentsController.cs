using Cs_backend.DTO;
using Cs_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[Authorize]
[ApiController]
[Route("groups/{groupId:int}")]
public class StudentsController(StudentService studentService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetStudentsByGroupId(int groupId)
    {
        var courses = await studentService.GetStudentsByGroupId(groupId);
        return new JsonResult(courses);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddOrUpdateStudent(int groupId, [FromBody] StudentDto dto)
    {
        var student = dto.Cast();
        student.GroupId = groupId;
        await studentService.AddOrUpdateStudent(student);
        return Ok(new { message = "Cтудент добавлен или обновлён" });
    }
}