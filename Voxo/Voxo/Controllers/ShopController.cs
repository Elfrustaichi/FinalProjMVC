using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Controllers
{
    public class ShopController : Controller
    {
        private readonly VoxoContext _context;

        public ShopController(VoxoContext context)
        {
            _context = context;
        }
        public IActionResult Index(List<int> brandId, List<int> categoryId,List<int> tagId,int discount=0,int page=1)
        {
            ShopViewModel model = new ShopViewModel
            {
                Brands = _context.Brands.Include(x => x.Products).ToList(),
                Categories = _context.Categories.Include(x => x.Products).ToList(),
                Tags = _context.Tags.Include(x => x.ProductTags).ThenInclude(x => x.Product).ToList(),
                ShopBanner = _context.Banners.FirstOrDefault(x => x.Type =="ShopBanner"),
                PopularProducts=_context.Products.Include(x=>x.ProductImages).Include(x=>x.Brand).ToList(),
            };
            var query = _context.Products.Include(x=>x.ProductImages).Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductTags).ThenInclude(x => x.Tag).AsQueryable();

            if (categoryId.Count>0)
            {
                query=query.Where(x=>categoryId.Contains(x.CategoryId));
            }
            if (tagId.Count > 0)
            {
                foreach (var item in tagId)
                {
                    query = query.Where(x => x.ProductTags.Select(x => x.Tag.Id).Contains(item));
                }
               
            }

            if (brandId.Count > 0)
            {
                query=query.Where(x=>brandId.Contains(x.BrandId));
            }

            if (discount > 0)
            {
                query = query.Where(x => x.DiscountPercent > discount);
            }

            model.Products= PaginatedList<Product>.Create(query, page, 12);

            return View(model);
        }
        public IActionResult Search()
        {
            SearchViewModel searchViewModel = new SearchViewModel();
            var products = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductTags).ThenInclude(x => x.Tag)
                .ToList();

            if(products.Any())
            {
                searchViewModel.IsFound = true;
            }
            else
            {
                searchViewModel.IsFound = false;
            }
            searchViewModel.Products = products;

            return View(searchViewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Search(SearchViewModel viewModel)
        {
            var query=  _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductTags).ThenInclude(x => x.Tag).AsQueryable();
                
            if (viewModel.Search!=null) 
            {
                query = query.Where(x => x.Name.Contains(viewModel.Search));
            }

            SearchViewModel searchViewModel = new SearchViewModel();
            var products=query.ToList();

            if(products.Any())
            {
                searchViewModel.IsFound=true;
            }
            else
            {
                searchViewModel.IsFound=false;
            }

            searchViewModel.Products = products;
            searchViewModel.Search = viewModel.Search;
            
            return View(searchViewModel);
            
        }
        
    }
}
