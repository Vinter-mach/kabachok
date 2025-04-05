using Cs_backend.Models;
using Cs_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;


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
