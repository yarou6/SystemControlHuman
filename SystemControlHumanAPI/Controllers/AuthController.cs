using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SystemControlHumanAPI.DB;
using SystemControlHumanAPI.DTO;
using SystemControlHumanAPI.Tools;

namespace SystemControlHumanAPI.Controllers;
[Route("api/[controller]")]
public class AuthController : Controller
{
    public SystemControlContext db { get; set; }

    public AuthController(SystemControlContext db)
    {
        this.db = db;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody]LoginRequestDTO request)
    {
        var credential = await db.Credentials.Include(x=> x.Role).FirstOrDefaultAsync(c=> c.Username == request.Username);
        if (credential == null)
            return Unauthorized();
        
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            credential.PasswordHash
        );

        if (!isValidPassword)
            return Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, credential.Username),
            new Claim(ClaimTypes.Role, credential.Role.Title),
            new Claim("EmployeeId", credential.EmployeeId.ToString()),
        };

        var key = JwtSettings.GetSymmetricSecurityKey();
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirensIn = 3600;

        var toker = new JwtSecurityToken(
            issuer: JwtSettings.ISSUER,
            audience: JwtSettings.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expirensIn),
            signingCredentials: creds
        );
        return Ok(new LoginResponseDTO()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(toker),
            ExpiresIn = expirensIn
        });
    }
  
    
    [Authorize]
    [HttpPost("profile")]
    public async Task<ActionResult<EmplRoleDTO>> Profile()
    {
        var employeeId = int.Parse(User.FindFirst("EmployeeId")!.Value);

        var employee = await db.Employees.Include(e => e.Credentials).ThenInclude(c => c.Role).FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
            return NotFound();

        return Ok(new EmplRoleDTO
        {
            EmployeeDto = new EmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
            },
            RoleDto = new RoleDTO
            {
                Title = employee.Credentials.Last().Role.Title
            },
        });
    }
}