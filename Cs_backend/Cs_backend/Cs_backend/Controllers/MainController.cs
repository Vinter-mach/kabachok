using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;


[Authorize]
[ApiController]
[Route("")]
public class MainController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Сервер работает!");
    }
}
