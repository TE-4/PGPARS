using Microsoft.AspNetCore.Mvc;

namespace PGPARS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
