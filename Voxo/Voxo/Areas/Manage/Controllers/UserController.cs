using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    public class UserController : Controller
    {
        private readonly VoxoContext _context;

        public UserController(VoxoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data=_context.AppUsers.ToList();
            return View(data);
        }
    }
}
