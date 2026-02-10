using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemControlHumanAPI.DB;
using SystemControlHumanAPI.DTO;

namespace SystemControlHumanAPI.Controllers;

[Route("api/[controller]")]
public class ShiftsController : Controller
{
    public SystemControlContext db { get; set; }

    public ShiftsController(SystemControlContext db)
    {
        this.db = db;
    }
    
    [HttpGet("")]
    public ActionResult<List<ShiftDTO>> Shifts()
    {
        List<ShEmplDTO> list = new List<ShEmplDTO>();
        foreach (Shift shift in db.Shifts.Include(s=>s.Employee))
        {
            list.Add(new ShEmplDTO()
            {
                Shift = new ShiftDTO()
                {
                    Id = shift.Id,
                    EmployeeId = shift.EmployeeId,
                    StartDateTime = shift.StartDateTime,
                    EndDateTime = shift.EndDateTime,
                    Description = shift.Description,
                },
                Employee = new EmployeeDTO()
                {
                    Id = shift.EmployeeId,
                    FirstName = shift.Employee.FirstName,
                    LastName = shift.Employee.LastName,
                }
            });
        }
        return Ok(list);   
    }
    
    [HttpGet("{id}")]
    public ActionResult<ShiftDTO> ShiftOnId(int id)
    {
        Shift shift = db.Shifts.Include(s=>s.Employee).FirstOrDefault(x => x.Id == id);
        return Ok(new ShEmplDTO()
        {
            Shift = new ShiftDTO()
            {
                Id = shift.Id,
                EmployeeId = shift.EmployeeId,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Description = shift.Description,
            },
            Employee = new EmployeeDTO()
            {
                Id = shift.EmployeeId,
                FirstName = shift.Employee.FirstName,
                LastName = shift.Employee.LastName,
                Position = shift.Employee.Position,
                HireDate = shift.Employee.HireDate,
                IsActive = shift.Employee.IsActive,
            }
        });
    }
    
    [HttpGet("employee/{id}")]
    public ActionResult<List<ShiftDTO>> ShiftEmployeeOnId(int id)
    {
        DateTime oldestDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0, 0));
        //List<Shift> list = db.Shifts.Where(x=>x.EmployeeId == id).ToList().Where(x => (DateTime.UtcNow.Subtract(x.StartDateTime)) > TimeSpan.FromDays(30)).ToList();
        //List<Shift> list = db.Shifts.Where(x => (DateTime.Now - x.StartDateTime).Days < 30 && x.EmployeeId == id).ToList();
        List<Shift> list = db.Shifts.Where(x => x.StartDateTime >= oldestDate && x.EmployeeId == id).ToList();
        List<ShiftDTO> listDTO = new List<ShiftDTO>();
        foreach (Shift shift in list)
        {
            listDTO.Add(new ShiftDTO()
            {
                Id = shift.Id,
                EmployeeId = shift.EmployeeId,
                StartDateTime =  shift.StartDateTime,  
                EndDateTime =  shift.EndDateTime,
                Description = shift.Description,
            });
        }
        return Ok(listDTO);
    }
    
    [HttpPost("")]
    public ActionResult AddShift(ShiftDTO shift)
    {
        if (db.Employees.FirstOrDefault(x => x.Id == shift.EmployeeId) == null)
            return BadRequest("Нет пассажира");
        if(shift.StartDateTime > shift.EndDateTime)
            return BadRequest("Ты тупой");

        db.Shifts.Add(new Shift()
        {
            EmployeeId = shift.EmployeeId,
            StartDateTime = shift.StartDateTime,
            EndDateTime = shift.EndDateTime,
            Description = shift.Description,
        });
        db.SaveChanges();
        return Ok();
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateShift(int id,  [FromBody]ShiftDTO shift)
    {
        Shift shiftToUpdate = db.Shifts.FirstOrDefault(x => x.Id == id);
        shiftToUpdate.StartDateTime = shift.StartDateTime;
        shiftToUpdate.EndDateTime = shift.EndDateTime;
        shiftToUpdate.Description = shift.Description;
        
        db.SaveChanges();
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteShift(int id)
    {
        try
        {
            db.Shifts.Remove(db.Shifts.First(x => x.Id == id));
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