using Microsoft.AspNetCore.Mvc;

namespace SystemControlHumanAPI.Controllers;

public class EmployeesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}