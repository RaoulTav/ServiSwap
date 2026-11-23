using Microsoft.AspNetCore.Mvc;
using ServiSwap.Exceptions;
using ServiSwap.Extensions;
using ServiSwap.Models;
using ServiSwap.Services;
using ServiSwap.ViewModels;

namespace ServiSwap.Controllers;

public class ServiceController(ServiceManager serviceManager, HomePageService homePageService) : Controller
{
    private readonly ServiceManager serviceManager = serviceManager;
    private readonly HomePageService homePageService = homePageService;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmHours(int? reqServiceId)
    {
        int? personId = HttpContext.Session.GetPersonId();

        if (personId is not int authenticatedPersonId)
        {
            return RedirectToAction("Login", "Person");
        }

        if (reqServiceId is not int requestedServiceId || requestedServiceId <= 0)
        {
            TempData["ErrorMessage"] = "La demande de service est invalide.";

            return RedirectToAction("RService", "Person");
        }

        try
        {
            Person requester = serviceManager.ConfirmHours(authenticatedPersonId, requestedServiceId);
            HttpContext.Session.SetAuthenticatedPerson(requester);
            TempData["SuccessMessage"] = "La prestation a été confirmée et les crédits ont été transférés.";
        }
        catch (ArgumentOutOfRangeException)
        {
            TempData["ErrorMessage"] = "Le nombre d'heures indiqué pour cette prestation est invalide.";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.ErrorCode switch
            {
                ErrorCode.NotServiceRequester => "Seul le demandeur de ce service peut confirmer les heures.",
                ErrorCode.NotEnoughCredits => "Votre solde est insuffisant pour confirmer cette prestation.",
                ErrorCode.InvalidStateTransaction => "Cette demande ne peut pas être confirmée dans son état actuel.",
                _ => "La confirmation de la prestation n'a pas pu être effectuée."
            };
        }
        catch (EntityNotFoundException ex) when (ex.EntityName == "Person")
        {
            HttpContext.Session.ClearAuthenticatedPerson();

            return RedirectToAction("Login", "Person");
        }
        catch (EntityNotFoundException ex) when (ex.EntityName == "ReqService")
        {
            TempData["ErrorMessage"] = "Cette demande a déjà été confirmée ou n'existe plus.";
        }

        return RedirectToAction("RService", "Person");
    }

    [HttpGet]
    public IActionResult MyServices()
    {
        int? personId = HttpContext.Session.GetPersonId();

        if (personId is not int authenticatedPersonId)
        {
            return RedirectToAction("Login", "Person");
        }

        try
        {
            List<Service> services = serviceManager.GetServicesByProvider(authenticatedPersonId);

            return View(services);
        } 
        catch (EntityNotFoundException ex) when (ex.EntityName == "Provider")
        {
            HttpContext.Session.ClearAuthenticatedPerson();

            return RedirectToAction("Login", "Person");
        }
    }

    [HttpGet]
    public IActionResult AddS()
    {
        int? personId = HttpContext.Session.GetPersonId();

        if (personId is not int)
        {
            return RedirectToAction("Login", "Person");
        }

        ServiceCreationViewModel serviceCreationViewModel = new()
        {
            Categories = homePageService.GetCategories()
        };

        return View(serviceCreationViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddS(ServiceCreationViewModel model)
    {
        int? personId = HttpContext.Session.GetPersonId();

        if (personId is not int authenticatedPersonId)
        {
            return RedirectToAction("Login", "Person");
        }

        if (!ModelState.IsValid || model.CategoryId is not int categoryId)
        {
            model.Categories = homePageService.GetCategories();

            return View(model);
        }

        try
        {
            serviceManager.OfferService(authenticatedPersonId, categoryId, model.ServiceName, model.ServiceDescription);

            TempData["SuccessMessage"] = "Le service a été ajouté.";

            return RedirectToAction(nameof(MyServices));
        }
        catch (EntityNotFoundException ex) when (ex.EntityName == "Person")
        {
            HttpContext.Session.ClearAuthenticatedPerson();

            return RedirectToAction("Login", "Person");
        }
        catch (EntityNotFoundException ex) when (ex.EntityName == "Category")
        {
            ModelState.AddModelError(nameof(model.CategoryId), "La catégorie sélectionnée n'existe pas.");
            model.Categories = homePageService.GetCategories();
            
            return View(model);
        }
    }
}