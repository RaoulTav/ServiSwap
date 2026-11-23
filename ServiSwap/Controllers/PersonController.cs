using Microsoft.AspNetCore.Mvc;
using ServiSwap.Exceptions;
using ServiSwap.Models;
using ServiSwap.Services;
using ServiSwap.ViewModels;

namespace ServiSwap.Controllers;

public class PersonController(PersonService personService) : Controller
{
    private readonly PersonService personService = personService;

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(AuthenticateViewModel authenticate)
    {
        if (ModelState.IsValid)
        {
            Person? person = personService.Authenticate(authenticate.Username, authenticate.Password);

            if (person is null)
            {
                ViewBag.NoAccount = "Nom d'utilisateur ou mot de passe incorrect.";

                return View();
            }

            HttpContext.Session.SetInt32("PersonId", person.PersonId);
            HttpContext.Session.SetString("Username", person.Username);
            HttpContext.Session.SetInt32("CreditHours", person.CreditHours);

            return RedirectToAction("Index", "Home");
        }

        return View(authenticate);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(PersonCreationViewModel register)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Person person = await personService.CreateUser(
                    register.Username,
                    register.Password,
                    register.Email,
                    register.LastName,
                    register.FirstName,
                    register.StreetNumber,
                    register.StreetName,
                    register.City
                );

                HttpContext.Session.SetInt32("PersonId", person.PersonId);
                HttpContext.Session.SetString("Username", person.Username);
                HttpContext.Session.SetInt32("CreditHours", person.CreditHours);

                return RedirectToAction("Index", "Home");
            } catch (BusinessException ex)
            {
                ViewBag.Error = ex.ErrorCode switch
                {
                    ErrorCode.UserAlreadyExist => "Ce nom d'utilisateur ou cet email est déjà utilisé par un autre utilisateur.",
                    ErrorCode.AddressNotFound => "L'adresse n'a pas pu être localisée.",
                    _ => "La création du compte n'a pas pu être effectuée."
                };
            }
        }

        return View(register);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RequestService(int serviceId)
    {
        int? personId = HttpContext.Session.GetInt32("PersonId");

        if (personId is null) return RedirectToAction("Login", "Person");
    
        try
        {
            personService.RequestService(personId.Value, serviceId);
            TempData["SuccessMessage"] = "La demande de service a été envoyée.";

            return RedirectToAction("ServiceDetails", "Service", new { serviceId });
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
    
            return RedirectToAction("ServiceDetails", "Service", new { serviceId }); 
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
