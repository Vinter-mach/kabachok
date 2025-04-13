using Cs_backend.DTO;
using Cs_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

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
    public async Task<IActionResult> AddOrUpdateTask(int groupId, [FromBody] StudentDto dto)
    {
        var student = dto.ToStudent();
        student.GroupId = groupId;
        await studentService.AddOrUpdateStudent(student);
        return Ok(new { message = "Cтудент добавлен или обновлён" });
    }
}