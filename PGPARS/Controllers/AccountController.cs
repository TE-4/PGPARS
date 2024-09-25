using Microsoft.AspNetCore.Mvc;

namespace PGPARS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
