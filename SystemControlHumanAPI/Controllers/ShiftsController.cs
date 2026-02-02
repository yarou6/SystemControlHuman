using Microsoft.AspNetCore.Mvc;

namespace SystemControlHumanAPI.Controllers;

public class ShiftsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}