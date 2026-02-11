using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemControlHumanAPI.DB;
using SystemControlHumanAPI.DTO;
using BCrypt.Net;

namespace SystemControlHumanAPI.Controllers;

[Route("api/[controller]")]
public class EmployeesController : Controller
{
    public SystemControlContext db { get; set; }

    public EmployeesController(SystemControlContext db)
    {
        this.db = db;
    }
    
    [HttpGet("")]
    public async Task<ActionResult<List<EmplRoleDTO>>> Employees()
    {
        var employees = await db.Employees.Include(s => s.Credentials).ThenInclude(s => s.Role).ToListAsync();

        List<EmplRoleDTO> list = new List<EmplRoleDTO>();
        foreach (Employee employee in employees)
        {
            list.Add(new EmplRoleDTO()
            {
                EmployeeDto = new EmployeeDTO()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Position =  employee.Position,
                    HireDate =  employee.HireDate,
                    IsActive =  employee.IsActive,
                },
                RoleDto = new RoleDTO()
                {
                    Title = employee.Credentials.Last().Role.Title
                }
            });
        }
        return Ok(list);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<EmplRoleDTO>> EmployeeOnId(int id)
    {
        Employee employee = await db.Employees.Include(s=>s.Credentials).ThenInclude(s=>s.Role).FirstOrDefaultAsync(x => x.Id == id);
        return Ok(new EmplRoleDTO()
        {
            EmployeeDto = new EmployeeDTO()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position =  employee.Position,
                HireDate =  employee.HireDate,
                IsActive =  employee.IsActive,
            },
            RoleDto = new RoleDTO()
            {
                Title = employee.Credentials.Last().Role.Title
            }
        });
    }
    
    [HttpPost("")]
    public  async Task<ActionResult> AddEmployee(CredEmplDTO employeeCred)
    {
        
        if (await db.Credentials.FirstOrDefaultAsync(x => x.Username == employeeCred.Credential.Username) != null)
            return BadRequest("Username already exists");
        
        Employee Employee = new Employee
        {
            FirstName = employeeCred.Employee.FirstName,
            LastName = employeeCred.Employee.LastName,
            Position = employeeCred.Employee.Position,
            HireDate = employeeCred.Employee.HireDate,
            IsActive = employeeCred.Employee.IsActive,
        }; 
        db.Employees.Add(Employee);
        await db.SaveChangesAsync();
        
        Credential Credential = new Credential
        {
            Username = employeeCred.Credential.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(employeeCred.Credential.PasswordHash),
            RoleId = employeeCred.Credential.RoleId,
            EmployeeId = Employee.Id,
        };
        db.Credentials.Add(Credential);
        await db.SaveChangesAsync();
        
        CredentialDTO credentialDto = new CredentialDTO()
        {
            Id =  Credential.Id,
            Username = Credential.Username,
            PasswordHash = Credential.PasswordHash,
            RoleId = Credential.RoleId,
            EmployeeId = Credential.EmployeeId,
        };
        
        return Created($"", credentialDto);
    }
    
    [HttpPut("{id}")]
    public  async Task<ActionResult> UpdateEmployee(int id,  [FromBody]EmployeeDTO employeeDTO)
    {
        Employee employee = await db.Employees.FirstOrDefaultAsync(x => x.Id == id);
        // Employee employee = db.Employees.FirstOrDefault(x => x.Id ==  employeeDTO.Id);
        employee.FirstName = employeeDTO.FirstName;
        employee.LastName = employeeDTO.LastName;
        employee.Position = employeeDTO.Position;
        employee.HireDate = employeeDTO.HireDate;
        employee.IsActive = employeeDTO.IsActive;

        await db.SaveChangesAsync();
        return Ok();
    } 
    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        try
        { 
            Employee employee = await db.Employees.FirstOrDefaultAsync(x => x.Id == id);
            db.Employees.Remove(employee);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        await db.SaveChangesAsync();
        return NoContent();
    }
}