using Microsoft.AspNetCore.Mvc;
using ServiSwap.Exceptions;
using ServiSwap.Services;

namespace ServiSwap.Controllers;

public class PersonController(PersonService personService) : Controller
{
    private readonly PersonService personService = personService;

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RequestService(int serviceId)
    {
        int? personId = HttpContext.Session.GetInt32("PersonId");

        if (personId is null) return RedirectToAction("Login", "Person"); // TODO : create corresponding action for Login
    
        try
        {
            personService.RequestService(personId.Value, serviceId);
            TempData["SuccessMessage"] = "La demande de service a été envoyée.";

            return RedirectToAction("Details", "Service", new { id = serviceId }); // TODO : create corresponding action
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.ErrorCode switch
            {
                ErrorCode.NotEnoughCredits => "Vous n'avez pas assez de crédits pour demander ce service.",
                ErrorCode.RequesterCannotBeProvider => "Vous ne pouvez pas demander votre propre service.",
                ErrorCode.ServiceAlreadyRequested => "Vous avez déjà demandé ce service.",
                _ => "La demande n'a pas pu être effectuée."
            };
    
            return RedirectToAction("Details", "Service", new { id = serviceId }); 
        }
        catch (EntityNotFoundException ex) when (ex.EntityName == "Service")
        {
            return NotFound();
        }
        catch (EntityNotFoundException)
        {
            HttpContext.Session.Remove("PersonId");
            
            return RedirectToAction("Login", "Person"); 
        }
    }
}
