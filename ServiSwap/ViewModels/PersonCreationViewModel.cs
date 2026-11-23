using System.ComponentModel.DataAnnotations;

namespace ServiSwap.ViewModels;

public class PersonCreationViewModel
{
    [Required(ErrorMessage = "Veuillez saisir un nom d'utilisateur.")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "Le nom d'utilisateur doit être entre 7 et 20 caractères.")]
    [RegularExpression(@"^[a-zA-Z0-9]{7,20}$", ErrorMessage = "Le nom d'utilisateur ne peut pas contenir de caractères spéciaux.")]
    [Display(Name = "Nom d'utilisateur")]
    public required string Username { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Veuillez saisir un mot de passe.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9])[\s\S]+$", 
        ErrorMessage = "Le mot de passe doit contenir au moins une lettre majuscule, un chiffre et un caractère spécial.")]
    [Display(Name = "Mot de passe")]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Veuillez ressaisir votre mot de passe.")]
    [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
    public required string Password2 { get; set; }

    [Required(ErrorMessage = "Veuillez saisir votre nom.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Le nom doit contenir au maximum 50 caractères.")]
    [Display(Name = "Nom")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Veuillez saisir votre prénom.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Le prénom doit contenir au maximum 50 caractères.")]
    [Display(Name = "Prénom")]
    public required string FirstName { get; set; }

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Veuillez saisir une adresse email.")]
    [StringLength(50, MinimumLength = 7, ErrorMessage = "L'adresse email doit contenir entre 7 et 50 caractères.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Veuillez saisir une adresse e-mail valide.")]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Veuillez saisir un numéro de rue.")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "Le numéro de rue doit contenir au maximum 10 caractères.")]
    [Display(Name = "Numéro de rue")]
    public required string StreetNumber { get; set; }

    [Required(ErrorMessage = "Veuillez saisir un nom de rue.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Le nom de rue doit contenir au maximum 50 caractères.")]
    [Display(Name = "Rue")]
    public required string StreetName { get; set; }

    [Required(ErrorMessage = "Veuillez saisir une ville.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "La ville doit contenir au maximum 50 caractères.")]
    [Display(Name = "Ville")]
    public required string City { get; set; }
}