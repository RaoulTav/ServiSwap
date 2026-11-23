using Microsoft.AspNetCore.Mvc;
using ServiSwap.Exceptions;
using ServiSwap.Extensions;
using ServiSwap.Models;
using ServiSwap.Services;

namespace ServiSwap.Controllers;

public class ServiceController(ServiceManager serviceManager) : Controller
{
    private readonly ServiceManager serviceManager = serviceManager;

    [HttpGet]
    public IActionResult ServiceDetails(int serviceId)
    {
        int? personId = HttpContext.Session.GetPersonId();

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ReportHours(int? reqServiceId, int? reportedHours)
    {
        int? personId = HttpContext.Session.GetPersonId();

        if (personId is not int authenticatedPersonId)
        {
            return RedirectToAction("Login", "Person");
        }

        if (reqServiceId is not int requestedServiceId || requestedServiceId <= 0 || reportedHours is not int hours)
        {
            TempData["ErrorMessage"] = "Veuillez fournir un numéro de demande et un nombre d'heures valides.";
            return RedirectToAction("PService", "Person");
        }

        try
        {
            serviceManager.ReportHours(authenticatedPersonId, requestedServiceId, hours);
            TempData["SuccessMessage"] = "Les heures prestées ont été enregistrées.";
        }
        catch (ArgumentOutOfRangeException)
        {
            TempData["ErrorMessage"] = "Le nombre d'heures ne peut pas être négatif.";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.ErrorCode switch
            {
                ErrorCode.NotServiceProvider => "Vous ne pouvez déclarer les heures que pour un service que vous fournissez.",
                ErrorCode.InvalidStateTransition => "Cette demande ne peut pas être mise à jour dans son état actuel.",
                _ => "Les heures prestées n'ont pas pu être enregistrées."
            };
        }
        catch (EntityNotFoundException ex) when (ex.EntityName == "Person")
        {
            HttpContext.Session.ClearAuthenticatedPerson();
            
            return RedirectToAction("Login", "Person");
        }
        catch (EntityNotFoundException ex) when (ex.EntityName == "ReqService")
        {
            TempData["ErrorMessage"] = "Cette demande de service est introuvable.";
        }

        return RedirectToAction("PService", "Person");
    }
}