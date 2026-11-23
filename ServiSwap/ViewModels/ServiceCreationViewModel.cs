using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiSwap.Models;

namespace ServiSwap.ViewModels;

public class ServiceCreationViewModel
{
    [Required(ErrorMessage = "Veuillez saisir un nom pour le service.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Le nom du service ne doit pas dépasser les 50 caractères.")]
    [Display(Name = "Nom du service")]
    public string ServiceName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Veuillez saisir une description du service.")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "La description du service ne doit pas dépasser les 1000 caractères.")]
    [Display(Name = "Description du service")]
    public string ServiceDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Veuillez choisir une catégorie.")]
    [Range(1, int.MaxValue, ErrorMessage = "Veuillez choisir une catégorie valide.")]
    [Display(Name = "Catégorie")]
    public int? CategoryId { get; set; }

    [BindNever]
    public List<Category> Categories { get; set; } = [];
}