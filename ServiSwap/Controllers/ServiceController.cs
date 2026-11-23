using Microsoft.AspNetCore.Mvc;
using ServiSwap.Exceptions;
using ServiSwap.Models;
using ServiSwap.Services;

namespace ServiSwap.Controllers;

public class ServiceController(ServiceManager serviceManager) : Controller
{
    private readonly ServiceManager serviceManager = serviceManager;

    [HttpGet]
    public IActionResult ServiceDetails(int serviceId)
    {
        int? personId = HttpContext.Session.GetInt32("PersonId");

        if (personId is null)
        {
            return RedirectToAction("Login", "Person");
        }

        try
        {
            Service service = serviceManager.GetService(serviceId);

            return View(service);
        } catch (EntityNotFoundException ex) when (ex.EntityName == "Service")
        {
            return NotFound();
        } 
    }
}