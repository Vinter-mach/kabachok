using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[ApiController]
[Route("groups")]
public class CroupController(GroupService groupService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var courses = await groupService.GetGroups();
        return new JsonResult(courses);
    }

    [HttpPost] 
    public async Task<IActionResult> AddOrUpdateCourse([FromBody] GroupDto dto)
    {
        await groupService.AddOrUpdateGroup(dto.Cast());

        return Ok(new { message = "Группа добавлена или обновлёна" });
    }
}