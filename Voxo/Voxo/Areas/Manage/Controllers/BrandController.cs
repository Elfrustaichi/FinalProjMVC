using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly VoxoContext _context;

        public BrandController(VoxoContext context)
        {
            _context = context;
        }
        //Brand index start
        public IActionResult Index(int page=1)
        {
            var query=_context.Brands.AsQueryable();

            return View(PaginatedList<Brand>.Create(query,page,7));
        }
        //Brand index end

        //Brand create start
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Brand brand)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.Brands.Any(x=>x.Name==brand.Name))
            {
                ModelState.AddModelError("Name","Brand is already exists");
                return View();
            }

            _context.Brands.Add(brand);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Brand create end
        //Brand delete start
        public IActionResult Delete(int id)
        {
            var existBrand = _context.Brands.Find(id);

            if(existBrand == null)
            {
                return StatusCode(404);
            }
            
            _context.Brands.Remove(existBrand);
            _context.SaveChanges();
            return StatusCode(200);
        }
        //Brand delete end

        //Brand edit start
        public IActionResult Edit(int id)
        {
            var existBrand=_context.Brands.Find(id);
            if(existBrand == null)
            {
                return StatusCode(404);
            }

            return View(existBrand);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Brand brand)
        {
            var existBrand=_context.Brands.Find(brand.Id);

            if(existBrand == null)
            {
                return View("Error");
            }

            if(!ModelState.IsValid)
            {
                return View(existBrand);
            }

            if(existBrand.Name!=brand.Name&&_context.Brands.Any(x=>x.Name==brand.Name))
            {
                ModelState.AddModelError("Name","Brand already exists");
                return View(existBrand);
            }

            existBrand.Name=brand.Name;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Brand edit end
    }
}
