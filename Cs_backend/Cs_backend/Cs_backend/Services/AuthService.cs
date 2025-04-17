using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cs_backend.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cs_backend.Services;

public class AuthService(AuthRepository teacherRepo, IOptions<JwtSettings> jwtOptions)
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<string?> LoginAsync(string login, string password)
    {
        var teacher = await teacherRepo.GetByLoginAsync(login);
        if (teacher == null) return null;
        
        if (!BCrypt.Net.BCrypt.Verify(password, teacher.PasswordHash))
            return null;

        // Генерация токена
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, teacher.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
