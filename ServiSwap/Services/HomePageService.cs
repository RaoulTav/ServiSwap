using ServiSwap.DAL.Interfaces;
using ServiSwap.Models;

namespace ServiSwap.Services;

public class HomePageService(ICategoryDAL categoryDAL, IServiceDAL serviceDAL)
{
    private readonly ICategoryDAL categoryDAL = categoryDAL;
    private readonly IServiceDAL serviceDAL = serviceDAL;

    public HomePageData GetHomePageData() => new(categoryDAL.FindAll(), serviceDAL.FindAll());

    public List<Category> GetCategories() => categoryDAL.FindAll();
}