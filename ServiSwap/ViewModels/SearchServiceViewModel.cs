using System.ComponentModel.DataAnnotations;
using ServiSwap.Models;

namespace ServiSwap.ViewModels;

public class SearchServiceViewModel
{
    public List<Category> Categories { get; init; } = [];
    public List<Service> Services { get; init; } = [];
}