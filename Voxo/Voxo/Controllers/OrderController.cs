using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Controllers
{
    public class OrderController : Controller
    {
        private readonly VoxoContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(VoxoContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //Checkout start
        public async Task<IActionResult> Checkout()
        {
            OrderViewModel viewModel = new OrderViewModel();
            viewModel.CheckoutItems = GenerateCheckoutItems();

            if (!viewModel.CheckoutItems.Any())
            {
                TempData["Error"] = "There is no item in cart";
                return RedirectToAction("Index","home");
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                viewModel.Order = new OrderCreateViewModel
                {
                    FullAddress = user.Adress,
                    Fullname=user.Fullname,
                    Email=user.Email, 
                };
            }
            
            viewModel.TotalPrice = viewModel.CheckoutItems.Any() ? viewModel.CheckoutItems.Sum(x => x.Price * x.Count) : 0;
            return View(viewModel);
        }
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CheckoutCreate(OrderCreateViewModel viewModel)
        {
            if(viewModel.PostalCode == null)
            {
                TempData["error"] = "You must enter postal code";
                return RedirectToAction("checkout");
            }
            OrderViewModel vm = new OrderViewModel();
            vm.CheckoutItems = GenerateCheckoutItems();
            vm.Order = viewModel;
            if (!User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrWhiteSpace(viewModel.Fullname))
                    ModelState.AddModelError("FullName", "FullName is required");

                if (string.IsNullOrWhiteSpace(viewModel.Email))
                    ModelState.AddModelError("Email", "Email is required");
                if (string.IsNullOrWhiteSpace(viewModel.FullAddress))
                    ModelState.AddModelError("Address", "Adress is required");
            }

            if (!ModelState.IsValid)
            {

                TempData["Error"] = "Checkout cannot created";
                return RedirectToAction("index", "home");
            }

            Order order = new Order
            {
                Fullname = viewModel.Fullname,
                FullAdress=viewModel.FullAddress,
                PostalCode=viewModel.PostalCode,
                Email=viewModel.Email,
                Note=viewModel.Note,
                CreateTime=DateTime.Now,
                Status=Enums.OrderStatus.Pending,
                TotalPrice=vm.CheckoutItems.Sum(x=>x.Price),
            };
            var items = GenerateCheckoutItems();
            foreach (var item in items)
            {
                Product product = _context.Products.Find(item.Product.Id);

                OrderItem orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    DiscountPercent = product.DiscountPercent,
                    UnitCostPrice = product.CostPrice,
                    UnitPrice = product.DiscountPercent > 0 ? (product.SalePrice * (100 - product.DiscountPercent) / 100) : product.SalePrice,
                    Count = item.Count,
                };

                order.OrderItems.Add(orderItem);
            }

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                order.Fullname = user.Fullname;
                order.Email = user.Email;
                order.AppUserId = user.Id;

                ClearDbBasket(user.Id);
            }
            else
            {
                order.Fullname = viewModel.Fullname;
                order.Email = viewModel.Email;

                Response.Cookies.Delete("Cart");
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("ordersuccess",new {orderId=order.Id});
        }

        private List<CheckoutViewModel> GenerateCheckoutItems()
        {
            if(User.Identity.IsAuthenticated)
            {
                string userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
                return GenerateCheckoutItemsDb(userId);
            }
            else
            {
                return GenerateCheckoutItemsCK();
            }
        }

        private List<CheckoutViewModel> GenerateCheckoutItemsDb(string userId)
        {
            return _context.UserCartItems.Include(x => x.Product).Where(x => x.AppUser.Id == userId).Select(y => new CheckoutViewModel
            {
                Count = y.Count,
                Name = y.Product.Name,
                Product = y.Product,
                Price = y.Product.DiscountPercent > 0 ? (y.Product.SalePrice * (100 - y.Product.DiscountPercent) / 100) : y.Product.SalePrice
            }).ToList();
        }
        private List<CheckoutViewModel> GenerateCheckoutItemsCK()
        {
            List<CheckoutViewModel> checkoutItems = new List<CheckoutViewModel>();
            var basketStr = Request.Cookies["basket"];

            if (basketStr != null)
            {
                List<CartItemCookieViewModel> cookieItems = JsonConvert.DeserializeObject<List<CartItemCookieViewModel>>(basketStr);

                foreach (var item in cookieItems)
                {
                    Product product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);

                    CheckoutViewModel checkoutItem = new CheckoutViewModel
                    {
                        Count = item.Count,
                        Name = product.Name,
                        Product = product,
                        Price = product.DiscountPercent > 0 ? (product.SalePrice * (100 - product.DiscountPercent) / 100) : product.SalePrice
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }

            return checkoutItems;
        }
        private void ClearDbBasket(string userId)
        {
            _context.UserCartItems.RemoveRange(_context.UserCartItems.Where(x => x.AppUserId == userId).ToList());
            _context.SaveChanges();
        }
        //Checkout end

        public IActionResult OrderSuccess(int orderId)
        {
            var orderExist = _context.Orders.Include(x=>x.OrderItems).ThenInclude(x=>x.Product).ThenInclude(x=>x.ProductImages).Include(x=>x.OrderItems).ThenInclude(x=>x.Product).ThenInclude(x=>x.Brand).FirstOrDefault(x => x.Id == orderId);
            return View(orderExist);
        }
    }
}
