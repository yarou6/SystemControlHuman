using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemControlHumanAPI.DB;
using SystemControlHumanAPI.DTO;

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
    public ActionResult<List<EmployeeDTO>> Employees()
    {
        List<EmplRoleDTO> list = new List<EmplRoleDTO>();
        foreach (Employee employee in db.Employees.Include(s=>s.Credentials).ThenInclude(s=>s.Role))
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
    public ActionResult<EmplRoleDTO> EmployeeOnId(int id)
    {
        Employee employee = db.Employees.Include(s=>s.Credentials).ThenInclude(s=>s.Role).FirstOrDefault(x => x.Id == id);
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
    public ActionResult AddEmployee(CredEmplDTO employeeCred)
    {
        
        if (db.Credentials.FirstOrDefault(x => x.Username == employeeCred.Credential.Username) != null)
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
        db.SaveChanges();
        
        Credential Credential = new Credential
        {
            Username = employeeCred.Credential.Username,
            PasswordHash = employeeCred.Credential.PasswordHash,
            RoleId = employeeCred.Credential.RoleId,
            EmployeeId = Employee.Id,
        };
        db.Credentials.Add(Credential);
        db.SaveChanges();
        
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
    public ActionResult UpdateEmployee(int id,  [FromBody]EmployeeDTO employeeDTO)
    {
        Employee employee = db.Employees.FirstOrDefault(x => x.Id == id);
        // Employee employee = db.Employees.FirstOrDefault(x => x.Id ==  employeeDTO.Id);
        employee.FirstName = employeeDTO.FirstName;
        employee.LastName = employeeDTO.LastName;
        employee.Position = employeeDTO.Position;
        employee.HireDate = employeeDTO.HireDate;
        employee.IsActive = employeeDTO.IsActive;

        db.SaveChanges();
        return Ok();
    } 
    
    
    [HttpDelete("{id}")]
    public ActionResult DeleteEmployee(int id)
    {
        try
        {
            db.Employees.Remove(db.Employees.First(x => x.Id == id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        db.SaveChanges();
        return NoContent();
    }
}