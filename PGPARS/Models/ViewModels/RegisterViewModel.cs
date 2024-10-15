using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [UIHint("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage= "Role is required!")]
        public string Role { get; set; }



    }
}
