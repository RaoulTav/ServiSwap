using Microsoft.AspNetCore.Mvc;
using ServiSwap.Services;

namespace ServiSwap.Controllers;

public class PersonController(PersonService personService) : Controller
{
    private readonly PersonService personService = personService;

    public IActionResult RequestService(int serviceId)
    {
        personService.RequestService(1, 3); // TODO: implement session and service view to update mockup data

        return View();
    }
}
