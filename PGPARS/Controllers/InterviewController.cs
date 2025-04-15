using Microsoft.AspNetCore.Mvc;

namespace PGPARS.Controllers
{
    public class InterviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
