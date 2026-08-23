using Microsoft.AspNetCore.Mvc;

namespace ServiSwap.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
