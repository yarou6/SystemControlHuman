using Microsoft.AspNetCore.Mvc;

namespace SystemControlHumanAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController : Controller
{
    [HttpPost("/api/auth/login")]
    public async Task Login(string username, string password)
    {
        return View();
    }
}