using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class ContactUsRequestController : Controller
    {
        private readonly VoxoContext _context;

        public ContactUsRequestController(VoxoContext context)
        {
            _context = context;
        }
        //Contact us request index start
        public IActionResult Index(int page=1)
        {
            var query=_context.ContactUsRequests.AsQueryable();

            return View(PaginatedList<ContactUsRequest>.Create(query,page,7));
        }
        //Contact us request index end

        //Contact us request delete start
        public IActionResult Delete(int id)
        {
            var existRequest=_context.ContactUsRequests.Find(id);

            if(existRequest == null)
            {
                return StatusCode(400);
            }

            _context.ContactUsRequests.Remove(existRequest);
            _context.SaveChanges();
            return StatusCode(200);
        }
        //Contact us request delete end
        //Contact us request edit start
        public IActionResult Edit(int id)
        {
            var existRequest= _context.ContactUsRequests.Find(id);
            if(existRequest == null)
            {
                return StatusCode(404);
            }

            return View(existRequest);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(ContactUsRequest request)
        {
            var existRequest = _context.ContactUsRequests.Find(request.Id);

            if(existRequest == null)
            {
                return View("error");
            }

            if(!ModelState.IsValid)
            {
                return View(request);
            }

            existRequest.ReplyText = request.ReplyText;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Contact us request edit end

    }
}
