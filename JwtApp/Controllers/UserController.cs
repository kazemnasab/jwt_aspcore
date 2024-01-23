using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IConfiguration _config;

    public UserController(IConfiguration config)
    {
        _config = config;
    }
    [HttpGet(Name = "login")]
    public IActionResult Get()
    {
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (_config["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim("Role", "Admin"),
                new Claim(JwtRegisteredClaimNames.Sub, "Mohammad"),
                new Claim(JwtRegisteredClaimNames.Email, "M@m.com"),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Ok(stringToken);
    }
}

