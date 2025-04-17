using Cs_backend.DTO;
using Cs_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var token = await authService.LoginAsync(login.Username, login.Password);
        if (token == null)
            return Unauthorized();

        return Ok(new { token });
    }
}
