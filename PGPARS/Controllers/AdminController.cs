using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models;

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