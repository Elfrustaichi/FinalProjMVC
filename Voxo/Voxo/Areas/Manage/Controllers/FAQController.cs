using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class FAQController : Controller
    {
        private readonly VoxoContext _context;

        public FAQController(VoxoContext context)
        {
            _context = context;
        }
        //FAQ index start
        public IActionResult Index(int page=1)
        {
            var query=_context.FAQs.AsQueryable();

            return View(PaginatedList<FAQ>.Create(query,page,7));
        }
        //FAQ index end
        //FAQ create start
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(FAQ faq)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.FAQs.Any(x=>x.QuestionText==faq.QuestionText))
            {
                ModelState.AddModelError("QuestionText","FAQ already exists");
                return View();
            }
            _context.FAQs.Add(faq);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //FAQ create end
        //FAQ delete start
        public IActionResult Delete(int id)
        {
            var existFaq = _context.FAQs.Find(id);

            if(existFaq==null)
            {
                return StatusCode(404);
            }
            _context.FAQs.Remove(existFaq);
            _context.SaveChanges();
            return StatusCode(200);
        }
        //FAQ delete end

        //FAQ edit start
        public IActionResult Edit(int id)
        {
            var existFaq=_context.FAQs.Find(id);

            if (existFaq == null)
            {
                return StatusCode(404);
            }

            return View(existFaq);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(FAQ Faq)
        {
            var existFaq=_context.FAQs.Find(Faq.Id);

            if (existFaq == null)
            {
                return View("Error");
            }
            if(!ModelState.IsValid)
            {
                return View(Faq);
            }
            if(existFaq.QuestionText!=Faq.QuestionText&&_context.FAQs.Any(x=>x.QuestionText==Faq.QuestionText))
            {
                ModelState.AddModelError("QuestionText", "FAQ already exists");
                return View();
            }

            existFaq.QuestionText= Faq.QuestionText;
            existFaq.AnswerText= Faq.AnswerText;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //FAQ edit end
    }
}
