using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    public class OrderController : Controller
    {
        private readonly VoxoContext _context;

        public OrderController(VoxoContext context)
        {
            _context = context;
        }
        //Order index start
        public IActionResult Index(int page=1)
        {
            var query=_context.Orders.Include(x=>x.AppUser).Include(x=>x.OrderItems).AsQueryable();

            return View(PaginatedList<Order>.Create(query,page,1));
        }
        //Order index end
        //Order delete start
        public IActionResult Delete(int id)
        {
            var existOrder = _context.Orders.Find(id);

            if (existOrder == null)
            {
                return StatusCode(404);
            }

            _context.Orders.Remove(existOrder);
            _context.SaveChanges();
            return StatusCode(200);
        }
        //Order delete end

        //Order edit start
        public IActionResult Edit(int id)
        {
            var existOrder=_context.Orders.Find(id);
            if(existOrder == null)
            {
                return StatusCode(404);
            }

            return View(existOrder);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Order order)
        {
            var existOrder = _context.Orders.Find(order.Id);

            if (existOrder == null)
            {
                return View("error");
            }
            

            if(order.Status!=Enums.OrderStatus.Accepted||order.Status!=Enums.OrderStatus.Rejected||order.Status!=Enums.OrderStatus.Pending)
            {
                return View("error");
            }

            existOrder.Status = order.Status;
            _context.SaveChanges();

            return RedirectToAction("index");

        }
        //Order edit end
        //Order detail start
        public IActionResult Detail(int id)
        {
            var existOrder=_context.Orders
                .Include(x=>x.AppUser)
                .Include(x=>x.OrderItems).ThenInclude(x=>x.Product).ThenInclude(x=>x.ProductImages)
                .Include(x=>x.Adress)
                .Include(x=>x.AppUser).ThenInclude(x=>x.Adresses)
                .FirstOrDefault(x=>x.Id==id);

            if(existOrder == null)
            {
                return StatusCode(404);
            }

            return View(existOrder);
        }
        //Order detail end
    }
}
