using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SizeController : Controller
    {
        private readonly VoxoContext _context;

        public SizeController(VoxoContext context)
        {
            _context = context;
        }
        //Size index start
        public IActionResult Index(int page=1)
        {
            var query=_context.Sizes.AsQueryable();

            return View(PaginatedList<Size>.Create(query,page,3));
        }
        //Size index end

        //Size create start
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Size size)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(_context.Sizes.Any(x=>x.Name==size.Name))
            {
                ModelState.AddModelError("Name","Size is already exists");
                return View();
            }

            _context.Sizes.Add(size);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Size create end

        //Size delete start
        public IActionResult Delete(int id)
        {
            var existSize = _context.Sizes.Find(id);
            if (existSize == null)
            {
                return StatusCode(404);
            }

            _context.Sizes.Remove(existSize);
            _context.SaveChanges();
            return StatusCode(200);
        }
        //Size delete end

        //Size edit start
        public IActionResult Edit(int id) 
        { 
            var existSize=_context.Sizes.Find(id);

            if(existSize == null)
            {
                return StatusCode(404);
            }
            return View(existSize);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Size size)
        {
            var existSize = _context.Sizes.Find(size.Id);

            if (existSize == null)
            {
                return View("Error");
            }
            if(!ModelState.IsValid)
            {
                return View(size);
            }
            if (existSize.Name != size.Name && _context.Sizes.Any(x => x.Name == size.Name))
            {
                ModelState.AddModelError("Name", "Size already exists");
                return View(size);
            }

            existSize.Name= size.Name;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Size edit end
    }
}
