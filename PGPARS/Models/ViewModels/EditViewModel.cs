using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models.ViewModels
{
    public class EditViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "N-Number")]
        public string Nnumber { get; set; }

        public string? Position { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }


        // Password Fields

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The password and confirmation password need to match!")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

    }
}
