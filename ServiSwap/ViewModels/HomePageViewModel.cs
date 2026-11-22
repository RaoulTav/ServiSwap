using ServiSwap.Models;

namespace ServiSwap.ViewModels;

public class HomePageViewModel
{
    public List<Category> Categories { get; init; } = [];
    public List<Service> Services { get; init; } = [];
}