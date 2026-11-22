using Microsoft.AspNetCore.Mvc;
using ServiSwap.Services;
using ServiSwap.ViewModels;

namespace ServiSwap.Controllers;

public class HomeController(HomePageService homePageService) : Controller
{
    private readonly HomePageService homePageService = homePageService;

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
}