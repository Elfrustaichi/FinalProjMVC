using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    public class TagController : Controller
    {
        private readonly VoxoContext _context;

        public TagController(VoxoContext context)
        {
            _context = context;
        }
        //Tag index start
        public IActionResult Index(int page=1)
        {
            var query=_context.Tags.AsQueryable();

            return View(PaginatedList<Tag>.Create(query,page,1));
        }
        //Tag index end

        //Tag create start
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Tag tag)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.Tags.Any(x=>x.Name==tag.Name))
            {
                ModelState.AddModelError("Name", "Tag is already exist");
                return View();
            }

            _context.Tags.Add(tag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Tag create end
        //Tag delete start
        public IActionResult Delete(int id)
        {
            var existTag = _context.Tags.Find(id);

            if(existTag==null)
            {
                return StatusCode(404);
            }
            _context.Remove(existTag);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Tag delete end

        //Tag edit start
        public IActionResult Edit(int id)
        {
            var existTag= _context.Tags.Find(id);

            if(existTag==null)
            {
                return StatusCode(404);
            }
            return View(existTag);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Tag tag)
        {
            var existTag=_context.Tags.Find(tag.Id);
            if(existTag==null)
            {
                return View("Error");
            }

            if(!ModelState.IsValid)
            {
                return View(existTag);
            }
            if (tag.Name != existTag.Name && _context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name","Tag is already exists");
                return View(existTag);
            }
            existTag.Name = tag.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        //Tag edit end
    }
}
