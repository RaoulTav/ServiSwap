using System.ComponentModel.DataAnnotations;

namespace ServiSwap.ViewModels;

public class AuthenticateViewModel
{
    [Required(ErrorMessage = "Veuillez saisir un nom d'utilisateur.")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "Le nom d'utilisateur doit être entre 7 et 20 caractères.")]
    [RegularExpression(@"^[a-zA-Z0-9]{7,20}$", ErrorMessage = "Le nom d'utilisateur ne peut pas contenir de caractères spéciaux.")]
    [Display(Name = "Nom d'utilisateur")]
    public required string Username { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Veuillez saisir un mot de passe.")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "Le mot de passe doit être entre 7 et 20 caractères.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{7,20}$", 
        ErrorMessage = "Le mot de passe doit contenir au moins une lettre majuscule, un chiffre et un caractère spécial.")]
    [Display(Name = "Mot de passe")]
    public required string Password { get; set; }
}