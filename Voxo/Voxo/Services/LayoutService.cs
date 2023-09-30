using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Services
{
    public class LayoutService
    {
        private readonly VoxoContext _context;
        private readonly IHttpContextAccessor _contextAccessor;


        public LayoutService(VoxoContext context,IHttpContextAccessor httpContextAccessor) 
        { 
            _context = context;
            _contextAccessor = httpContextAccessor;
        }

        public Dictionary<string,string> GetSettings()
        {
            return _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);
        }
        public List<Category> GetCategories()
        {
            return _context.Categories.Where(x => x.CategoryTag == "New").Take(5).ToList();
        }

        public CartViewModel GetCart()
        {
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var cartItems = _context.UserCartItems.Include(x=>x.Product).ThenInclude(x=>x.ProductImages).Where(x => x.AppUserId == _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
                var viewModel = new CartViewModel();
                foreach (var item in cartItems)
                {
                    CartItemViewModel cartItem = new CartItemViewModel
                    {
                        Product=item.Product,
                        Count=item.Count,
                    };
                    viewModel.Items.Add(cartItem);
                    viewModel.TotalPrice+= (item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (100 - item.Product.DiscountPercent) / 100) : item.Product.SalePrice) * item.Count;
                }
                return viewModel;
            }
            else
            {
                var viewModel= new CartViewModel();

                var cartCookie = _contextAccessor.HttpContext.Request.Cookies["Cart"];

                if(cartCookie != null)
                {
                    var cookieItems=JsonConvert.DeserializeObject<List<CartItemCookieViewModel>>(cartCookie);

                    foreach (var item in cookieItems)
                    {
                        CartItemViewModel cartItem = new CartItemViewModel
                        {
                            Product=_context.Products.FirstOrDefault(x=>x.Id==item.ProductId),
                            Count=item.Count,
                        };
                        viewModel.Items.Add(cartItem);
                        viewModel.TotalPrice+= (cartItem.Product.DiscountPercent > 0 ? (cartItem.Product.SalePrice * (100 - cartItem.Product.DiscountPercent) / 100) : cartItem.Product.SalePrice) * item.Count;
                    }
                    
                }
                return viewModel;
            }
        }

    }
}
