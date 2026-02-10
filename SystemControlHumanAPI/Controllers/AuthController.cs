using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemControlHumanAPI.DB;
using SystemControlHumanAPI.DTO;

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
    public async Task<ActionResult> Login(string username, string password)
    {
        Credential credential = await db.Credentials.FirstOrDefaultAsync(s=>s.Username == username && s.PasswordHash == password);

        return Ok(credential);
    }

    [HttpPost("profile")]
    public ActionResult<EmployeeDTO> Profile(int id)
    {
        Employee employee = db.Employees.FirstOrDefault(x => x.Id == id);
        return Ok( new EmplRoleDTO()
            {
                EmployeeDto = new EmployeeDTO
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Position = employee.Position,
                },
                RoleDto = new RoleDTO()
                {
                    Title = db.Credentials.FirstOrDefault(x => x.EmployeeId == employee.Id).Role.Title,
                }
            }
        );
    }
    
    
    
    
}