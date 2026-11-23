using ServiSwap.Models;

namespace ServiSwap.Services;

public record HomePageData(List<Category> Categories, List<Service> Services);