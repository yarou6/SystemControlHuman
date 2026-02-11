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
    public async Task<ActionResult<List<ShiftDTO>>> Shifts()
    {
        var shifts= await db.Shifts.Include(s => s.Employee).ToListAsync();

        List<ShEmplDTO> list = new List<ShEmplDTO>();
        foreach (Shift shift in shifts)
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
    public async Task<ActionResult<ShiftDTO>> ShiftOnId(int id)
    {
        Shift shift = await db.Shifts.Include(s=>s.Employee).FirstOrDefaultAsync(x => x.Id == id);
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
    public async Task<ActionResult<List<ShiftDTO>>> ShiftEmployeeOnId(int id)
    {
        DateTime oldestDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0, 0));
        //List<Shift> list = db.Shifts.Where(x=>x.EmployeeId == id).ToList().Where(x => (DateTime.UtcNow.Subtract(x.StartDateTime)) > TimeSpan.FromDays(30)).ToList();
        //List<Shift> list = db.Shifts.Where(x => (DateTime.Now - x.StartDateTime).Days < 30 && x.EmployeeId == id).ToList();
        List<Shift> list = await db.Shifts.Where(x => x.StartDateTime >= oldestDate && x.EmployeeId == id).ToListAsync();
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
    public  async Task<ActionResult> AddShift(ShiftDTO shift)
    {
        if (await db.Employees.FirstOrDefaultAsync(x => x.Id == shift.EmployeeId) == null)
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
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateShift(int id,  [FromBody]ShiftDTO shift)
    {
        Shift shiftToUpdate = await db.Shifts.FirstOrDefaultAsync(x => x.Id == id);
        shiftToUpdate.StartDateTime = shift.StartDateTime;
        shiftToUpdate.EndDateTime = shift.EndDateTime;
        shiftToUpdate.Description = shift.Description;
        
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteShift(int id)
    {
        try
        {
            Shift shift = await db.Shifts.FirstOrDefaultAsync(x => x.Id == id);
            db.Shifts.Remove(shift);
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