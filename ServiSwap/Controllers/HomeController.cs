using Microsoft.AspNetCore.Mvc;
using ServiSwap.Exceptions;
using ServiSwap.Models;
using ServiSwap.Services;
using ServiSwap.ViewModels;

namespace ServiSwap.Controllers;

public class HomeController(HomePageService homePageService, ServiceManager serviceManager) : Controller
{
    private readonly HomePageService homePageService = homePageService;
    private readonly ServiceManager serviceManager = serviceManager;

    public IActionResult Index()
    {
        HomePageData data = homePageService.GetHomePageData();

        HomePageViewModel homePageViewModel = new()
        {
            Categories = data.Categories,
            Services = data.Services
        };

        return View(homePageViewModel);
    }

    [HttpGet]
    public IActionResult SearchForServices([Bind(Prefix = "Search")] ServiceSearchViewModel search)
    {
        int? personId = HttpContext.Session.GetInt32("PersonId");

        try
        {
            List<Service> services = ModelState.IsValid ? serviceManager.SearchServices(search.CategoryId, personId, search.Distance) : [];

            return View("Index", new HomePageViewModel
            {
                Search = search,
                Categories = homePageService.GetCategories(),
                Services = services
            });
        } catch (EntityNotFoundException ex) when (ex.EntityName == "Person")
        {
            HttpContext.Session.Remove("PersonId");

            return RedirectToAction("Login", "Person");
        }
    }
}