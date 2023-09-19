using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class ProductReviewController : Controller
    {
        private readonly VoxoContext _context;

        public ProductReviewController(VoxoContext context)
        {
            _context = context;
        }
        //ProductReview index start
        public IActionResult Index(int page=1)
        {
            var query=_context.ProductReviews.Include(x=>x.Product).Include(x=>x.AppUser).AsQueryable();

            return View(PaginatedList<ProductReview>.Create(query,page,7));
        }
        //ProductReview index end
        //Product delete start
        public IActionResult Delete(int id)
        {
            var existReview = _context.ProductReviews.Find(id);

            if(existReview == null)
            {
                return StatusCode(404);
            }

            _context.ProductReviews.Remove(existReview);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //ProductReview delete end
        //ProductReview edit start
        public IActionResult Edit(int id)
        {
            var existReview = _context.ProductReviews.Find(id);

            if(existReview == null)
            {
                return StatusCode(404);
            }

            return View(existReview);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(ProductReview review)
        {
            var existReview=_context.ProductReviews.Find(review.Id);

            if(existReview == null)
            {
                return View("error");
            }

            if(review.IsPublised!=true&&review.IsPublised!=false)
            {
                ModelState.AddModelError("IsPublished", "Publish value is wrong");
                return View(existReview);
            }
            
            if(review.AdminResponse!=null)
            {
                existReview.AdminResponse=review.AdminResponse;
            }

            existReview.IsPublised=review.IsPublised;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //ProductReview edit end

        //Product Review detail
        public IActionResult Detail(int id)
        {
            var existReview = _context.ProductReviews.Include(x=>x.AppUser).Include(x=>x.Product).ThenInclude(x=>x.ProductImages).FirstOrDefault(x=>x.Id==id);

            if(existReview == null)
            {
                return StatusCode(404);
            }

            return View(existReview);
        }
    }
}
