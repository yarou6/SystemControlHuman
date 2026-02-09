using Microsoft.AspNetCore.Mvc;
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
    
    [HttpGet("employees")]
    public ActionResult<List<EmployeeDTO>> Employees()
    {
        List<EmplRoleDTO> list = new List<EmplRoleDTO>();
        foreach (Employee employee in db.Employees)
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
                    Title = db.Credentials.FirstOrDefault(x => x.EmployeeId == employee.Id).Role.Title,
                }
            });
        }
        return Ok(list);
    }
    
    [HttpGet("employees/{id}")]
    public ActionResult<EmployeeDTO> EmployeeOnId(int id)
    {
        Employee employee = db.Employees.FirstOrDefault(x => x.Id == id);
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
                Title = db.Credentials.FirstOrDefault(x => x.EmployeeId == employee.Id).Role.Title,
            }
        });
    }
    
    [HttpPost("employees")]
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
            EmployeeId = db.Employees.Last().Id,
        };
        db.Credentials.Add(Credential);
        db.SaveChanges();
        
        Credential credential = db.Credentials.Last();
        CredentialDTO credentialDto = new CredentialDTO()
        {
            Username = credential.Username,
            PasswordHash = credential.PasswordHash,
            RoleId = credential.RoleId,
            EmployeeId = credential.EmployeeId,
        };
        
        return Created($"", credentialDto);
    }
    
    [HttpPut("employees/{id}")]
    public ActionResult UpdateEmployee(int Id,  EmployeeDTO employeeDTO)
    {
        Employee employee = db.Employees.FirstOrDefault(x => x.Id == Id);
        employee.FirstName = employeeDTO.FirstName;
        employee.LastName = employeeDTO.LastName;
        employee.Position = employeeDTO.Position;
        employee.HireDate = employeeDTO.HireDate;
        employee.IsActive = employeeDTO.IsActive;
        
        db.SaveChanges();
        return Ok();
    } 
    
    
    [HttpDelete("employees/{id}")]
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