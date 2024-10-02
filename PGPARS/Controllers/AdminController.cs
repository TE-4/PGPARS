using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models;
using Microsoft.AspNetCore.Authorization;

namespace PGPARS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}