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
        List<EmployeeDTO> list = new List<EmployeeDTO>();
        foreach (Employee employee in db.Employees)
        {
            list.Add(new EmployeeDTO()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position =  employee.Position,
                HireDate =  employee.HireDate,
                IsActive =  employee.IsActive,
                Role = db.Credentials.FirstOrDefault(x => x.EmployeeId == employee.Id).Role
            });
        }
        return Ok(list);
    }
    
    [HttpGet("employees/{id}")]
    public ActionResult<EmployeeDTO> EmployeeOnId(int id)
    {
        Employee employee = db.Employees.FirstOrDefault(x => x.Id == id);
        return Ok(new EmployeeDTO()
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Position =  employee.Position,
            HireDate =  employee.HireDate,
            IsActive =  employee.IsActive,
            Role = db.Credentials.FirstOrDefault(x => x.EmployeeId == employee.Id).Role
        });
    }
    
    [HttpPost("employees")]
    public ActionResult AddEmployee()
    {
        
        return View();
    }
    
    [HttpPut("employees/{id}")]
    public ActionResult UpdateEmployee()
    {
        
        return View();
    }
    
    [HttpDelete("employees/{id}")]
    public ActionResult DeleteEmployee()
    {
        
        return View();
    }
}