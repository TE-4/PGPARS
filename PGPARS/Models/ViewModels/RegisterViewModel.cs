using Microsoft.AspNetCore.Mvc.Rendering;
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
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }



    }
}
