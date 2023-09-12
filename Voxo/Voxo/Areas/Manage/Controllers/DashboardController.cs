using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Voxo.Models;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


    }
}
