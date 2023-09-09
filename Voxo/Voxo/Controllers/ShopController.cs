using Microsoft.AspNetCore.Mvc;

namespace Voxo.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }
    }
}
