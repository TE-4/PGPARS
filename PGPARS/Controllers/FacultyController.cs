using Microsoft.AspNetCore.Mvc;
using PGPARS.Models.ViewModels;

namespace PGPARS.Controllers
{
    public class FacultyController : Controller
    {
      public async Task<IActionResult> Dashboard()
        {
            return View();
        }

    }
}
