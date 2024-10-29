using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models.ViewModels
{
    public class EditViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "N-Number")]
        public string Nnumber { get; set; }
        [Required]
        public string? Position { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }

        [Required(ErrorMessage = "Current Password is required!")]
        [DataType(DataType.Password)]
        public string CurrPassword { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
