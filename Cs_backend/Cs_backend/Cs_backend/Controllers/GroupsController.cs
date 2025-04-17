using Cs_backend.DTO;
using Cs_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[Authorize]
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