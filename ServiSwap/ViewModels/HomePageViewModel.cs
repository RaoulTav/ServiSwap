using ServiSwap.Models;

namespace ServiSwap.ViewModels;

public class HomePageViewModel
{
    public ServiceSearchViewModel Search { get; init; } = new();

    public List<Category> Categories { get; init; } = [];
    public List<Service> Services { get; init; } = [];
}