using Microsoft.AspNetCore.Mvc;
using SystemControlHumanAPI.DB;

namespace SystemControlHumanAPI.Controllers;

[Route("api/[controller]")]
public class ShiftsController : Controller
{
    public SystemControlContext db { get; set; }

    public ShiftsController(SystemControlContext db)
    {
        this.db = db;
    }
    
    [HttpGet("shifts")]
    public IActionResult Shifts()
    {
        
        return View();
    }
    
    [HttpGet("shifts/{id}")]
    public IActionResult ShiftOnId()
    {
        
        return View();
    }
    
    [HttpGet("shifts/employee/{id}")]
    public IActionResult ShiftEmployeeOnId()
    {
        
        return View();
    }
    
    [HttpPost("shifts")]
    public IActionResult AddShift()
    {
        
        return View();
    }
    
    [HttpPut("shifts/{id}")]
    public IActionResult UpdateShift()
    {
        
        return View();
    }
    
    [HttpDelete("shifts/{id}")]
    public IActionResult DeleteShift()
    {
        
        return View();
    }
}