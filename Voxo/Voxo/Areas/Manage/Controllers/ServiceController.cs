using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Helpers;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ServiceController : Controller
    {
        private readonly VoxoContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(VoxoContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Service index start
        public IActionResult Index(int page=1)
        {
            var query = _context.Services.AsQueryable();

            return View(PaginatedList<Service>.Create(query,page,1));
        }
        //Service index end
        //Service create start
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Service service)
        {
           if(!ModelState.IsValid)
            {
                return View();
            } 

           if(_context.Services.Any(x=>x.Header==service.Header))
            {
                ModelState.AddModelError("Header", "Service already exists");
                return View();
            }

            service.Icon = FileManager.Save(_env.WebRootPath,"uploads/ServiceIcons",service.IconImageFile);

           _context.Services.Add(service);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Service create end
        //Service delete start
        public IActionResult Delete(int id)
        {
            var existService = _context.Services.Find(id);
            if(existService==null)
            {
                return StatusCode(404);
            }
            string oldImage = existService.Icon;

            _context.Services.Remove(existService);
            _context.SaveChanges();
            if (oldImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/ServiceIcons", oldImage);
            }
            
            return StatusCode(200);
        }
        //Service delete end

        //Service edit start
        public IActionResult Edit(int id)
        {
            var existService=_context.Services.Find(id);
            if (existService == null)
            {
                return StatusCode(404);
            }

            return View(existService);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Service service)
        {
            var existService = _context.Services.Find(service.Id);

            if (existService==null)
            {
                return View("Error");
            }

            if(!ModelState.IsValid)
            {
                return View(service);
            }

            if (existService.Header != service.Header && _context.Services.Any(x => x.Header == service.Header))
            {
                ModelState.AddModelError("Header","Service already exists");
                return View(service);
            }

            existService.Header = service.Header;
            existService.Text = service.Text;
            existService.Type = service.Type;

            string oldImage = null;
            if (service.IconImageFile!=null)
            {
                oldImage = existService.Icon;
                existService.Icon=FileManager.Save(_env.WebRootPath,"uploads/ServiceIcons",service.IconImageFile);

            }
            _context.SaveChanges();

            if(oldImage!=null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/ServiceIcons", oldImage);
            }
            

            return RedirectToAction("index");
        }
        //Service edit end
    }
}
