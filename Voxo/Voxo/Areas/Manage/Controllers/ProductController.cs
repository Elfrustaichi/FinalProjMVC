using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Voxo.DAL;
using Voxo.Helpers;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ProductController : Controller
    {
        private readonly VoxoContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(VoxoContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Product index start
        public IActionResult Index(int page=1)
        {
            var query=_context.Products
                .Include(x=>x.Brand)
                .Include(x=>x.Category)
                .Include(x=>x.ProductImages)
                .Include (x=>x.ProductReviews)
                .Include(x=>x.ProductSizes)
                .Include(x=>x.ProductTags).AsQueryable();

            return View(PaginatedList<Product>.Create(query,page,1));
        }
        //Product index end

        //Product create start
        public IActionResult Create()
        {
            GetPageDetails();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Product product)
        {
            GetPageDetails();
            if (!ModelState.IsValid)
            {

                return View();
            }
            if (!_context.Brands.Any(x => x.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found");

                return View();
            }
            if (!_context.Categories.Any(x => x.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found");

                return View();
            }
            foreach (var item in product.TagIds)
            {
                if (!_context.Tags.Any(x => x.Id == item))
                {
                    ModelState.AddModelError("TagIds", "Tag not found");

                    return View();

                }
            }
            foreach (var item in product.SizeIds)
            {
                if (!_context.Sizes.Any(x => x.Id == item))
                {
                    ModelState.AddModelError("SizeIds", "Size not found");

                    return View();
                }
            }
            if (_context.Products.Any(x => x.Name == product.Name))
            {

                ModelState.AddModelError("Name", "Product already exists");
                return View(product);
            }
            if (product.SalePrice <= product.CostPrice)
            {

                ModelState.AddModelError("SalePrice", "SalePrice cant be lower than cost price");
                return View(product);
            }
            if ((product.SalePrice - (product.SalePrice * product.DiscountPercent / 100)) <= product.CostPrice)
            {
                ModelState.AddModelError("DiscountPercent", "Discounted saleprice cant be lower than cost price");
                return View(product);
            }
            if (product.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage","Product must have poster image");
                return View(product);
            }
            if(product.HoverImage == null)
            {
                ModelState.AddModelError("HoverImage","Product must have hover image");
                return View(product);
            }
            if(product.DetailImages.Count==0)
            {
                ModelState.AddModelError("DetailImages","Product must have at least 1 detail image");
                return View(product);
            }

            //Create size start
            foreach (var item in product.SizeIds)
            {
                ProductSize productSize = new ProductSize()
                {
                    SizeId = item
                };
                product.ProductSizes.Add(productSize);
            }
            //Create size end
            //Create tag start
            foreach (var item in product.TagIds)
            {
                ProductTag productTag = new ProductTag()
                {
                     TagId= item
                };
                product.ProductTags.Add(productTag);
            }
            //Create tag end
            //Create images start
            ProductImage posterImage = new ProductImage()
            {
                PosterStatus = true,
                ImageName = FileManager.Save(_env.WebRootPath,"uploads/ProductImages",product.PosterImage),
                DetailPicture=false
            };
            product.ProductImages.Add(posterImage);

            ProductImage hoverImage = new ProductImage()
            {
                PosterStatus= false,
                ImageName= FileManager.Save(_env.WebRootPath,"uploads/ProductImages",product.HoverImage),
                DetailPicture=false
            };
            product.ProductImages.Add(hoverImage);
            foreach (var img in product.DetailImages)
            {
                ProductImage detailImage = new ProductImage()
                {
                    ImageName = FileManager.Save(_env.WebRootPath,"uploads/ProductImages",img),
                    ProductId=product.Id,
                    DetailPicture=true
                };
                product.ProductImages.Add(detailImage);
            }
            //Create images end
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        //Product create end
        //Product delete start
        public IActionResult Delete(int id)
        {
            
            var existsProduct = _context.Products?.Include(x => x.ProductImages).FirstOrDefault(x=>x.Id==id);
            if (existsProduct == null)
            {
                return StatusCode(404);
            }

            FileManager.Delete(_env.WebRootPath,"uploads/ProductImages", existsProduct.ProductImages.FirstOrDefault(x => x.PosterStatus == true).ImageName);
            FileManager.Delete(_env.WebRootPath,"uploads/ProductImages", existsProduct.ProductImages.FirstOrDefault(x => x.PosterStatus == false&&x.DetailPicture==false).ImageName);

            List<string> detailImages = new List<string>();
            foreach (var img in existsProduct.ProductImages.Where(x=>x.DetailPicture==true))
            {
                detailImages.Add(img.ImageName);
            }
            FileManager.DeleteAll(_env.WebRootPath,"uploads/ProductImages",detailImages);
            

            _context.Products.Remove(existsProduct);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Product delete end
        //Product edit start
        public IActionResult Edit(int id)
        {
            GetPageDetails();
            var existProduct = _context.Products.Include(x=>x.ProductImages).Include(x=>x.ProductTags).Include(x=>x.ProductSizes).FirstOrDefault(x=>x.Id==id);
            if (existProduct==null)
            {
                return StatusCode(404);
            }
            existProduct.TagIds=existProduct.ProductTags.Select(x=>x.TagId).ToList();
            existProduct.SizeIds=existProduct.ProductSizes.Select(x=>x.SizeId).ToList();

            return View(existProduct);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Product product)
        {
            GetPageDetails();
            var existProduct= _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductReviews)
                .Include(x => x.ProductSizes)
                .Include(x => x.ProductTags).FirstOrDefault(x=>x.Id==product.Id);
            if (existProduct==null)
            {
                return View("error");
            }
            
            if (!ModelState.IsValid)
            {
                return View(existProduct);
            }
            if (!_context.Brands.Any(x => x.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found");

                return View(existProduct);
            }
            if (!_context.Categories.Any(x => x.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found");

                return View(existProduct);
            }
            if(product.DetailImageIds.Count()<1)
            {
                ModelState.AddModelError("DetailImageIds", "Product must have at leats one detail image");
                return View(existProduct);
            }
            foreach (var item in product.TagIds)
            {
                if (!_context.Tags.Any(x => x.Id == item))
                {
                    ModelState.AddModelError("TagIds", "Tag not found");

                    return View(existProduct);

                }
            }
            foreach (var item in product.SizeIds)
            {
                if (!_context.Sizes.Any(x => x.Id == item))
                {
                    ModelState.AddModelError("SizeIds", "Size not found");

                    return View(existProduct);
                }
            }
            if (existProduct.Name!=product.Name&&_context.Products.Any(x => x.Name == product.Name))
            {

                ModelState.AddModelError("Name", "Product already exists");
                return View(existProduct);
            }
            if (product.SalePrice <= product.CostPrice)
            {

                ModelState.AddModelError("SalePrice", "SalePrice cant be lower than cost price");
                return View(existProduct);
            }
            if ((product.SalePrice - (product.SalePrice * product.DiscountPercent / 100)) <= product.CostPrice)
            {
                ModelState.AddModelError("DiscountPercent", "Discounted saleprice cant be lower than cost price");
                return View(existProduct);
            }
            //Edit size start
            existProduct.ProductSizes.RemoveAll(x=>!product.SizeIds.Contains(x.SizeId));

            var newProductSizes = product.SizeIds.FindAll(x => !existProduct.ProductSizes.Any(y => y.SizeId == x));

            foreach (var item in newProductSizes)
            {
                ProductSize productSize = new ProductSize()
                {
                    SizeId = item
                };
            existProduct.ProductSizes.Add(productSize);
            }
            //Edit size end
            //Edit tag start
            existProduct.ProductTags.RemoveAll(x => !product.TagIds.Contains(x.TagId));

            var newProductTags = product.TagIds.FindAll(x => !existProduct.ProductTags.Any(y => y.TagId == x));

            foreach (var item in newProductTags)
            {
                ProductTag productTag = new ProductTag()
                {
                    TagId = item
                };
                existProduct.ProductTags.Add(productTag);
            }
            //Edit tag end
            //Edit images start
            string oldPosterImage = null;
            string oldHoverImage = null;
            //Edit poster image start

            if (product.PosterImage != null)
            {
                ProductImage posterImage = existProduct.ProductImages.FirstOrDefault(x => x.PosterStatus == true);
                oldPosterImage = posterImage?.ImageName;

                if (posterImage == null)
                {
                    ProductImage newPosterImage = new ProductImage()
                    {
                        ImageName = FileManager.Save(_env.WebRootPath, "uploads/ProductImages", product.PosterImage),
                        PosterStatus = true,
                        DetailPicture = false
                    };
                    existProduct.ProductImages.Add(newPosterImage);
                }
                else
                {
                    posterImage.ImageName = FileManager.Save(_env.WebRootPath, "uploads/ProductImages", product.PosterImage);
                }


                
            }
            //Edit poster image end
            //Edit hover image start
            if (product.HoverImage != null)
            {
                ProductImage hoverImage=existProduct.ProductImages.FirstOrDefault(x=>x.PosterStatus== false&&x.DetailPicture==false);
                oldHoverImage=hoverImage.ImageName;

                if(hoverImage == null)
                {
                    ProductImage newHoverImage = new ProductImage()
                    {
                        ImageName = FileManager.Save(_env.WebRootPath, "uploads/ProductImages", product.HoverImage),
                        PosterStatus = false,
                        DetailPicture = false
                    };
                    existProduct.ProductImages.Add(newHoverImage);
                }
                else
                {
                    hoverImage.ImageName = FileManager.Save(_env.WebRootPath, "uploads/ProductImages", product.HoverImage);
                }
               
            }
            //Edit hover image end
            //Edit detail images start
            var deletedDetailImages = existProduct.ProductImages.FindAll(x => x.DetailPicture == true && !product.DetailImageIds.Contains(x.Id));
            existProduct.ProductImages.RemoveAll(x=>x.DetailPicture==true&&!product.DetailImageIds.Contains(x.Id));
            if(product.DetailImages!=null)
            {
                foreach (var item in product.DetailImages)
                {
                    ProductImage newDetailImage = new ProductImage()
                    {
                        DetailPicture = true,
                        ImageName = FileManager.Save(_env.WebRootPath, "uploads/ProductImages", item),
                        PosterStatus = false,
                    };
                    existProduct.ProductImages.Add(newDetailImage);
                }
            }

            //Edit detail images end

            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.CategoryId = product.CategoryId;
            existProduct.BrandId = product.BrandId;
            existProduct.SalePrice= product.SalePrice;
            existProduct.CostPrice= product.CostPrice;
            existProduct.DiscountPercent= product.DiscountPercent;
            existProduct.StockStatus= product.StockStatus;

            _context.SaveChanges();

            if(oldHoverImage != null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/ProductImages",oldHoverImage);
            }
            if (oldPosterImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/ProductImages", oldPosterImage);
            }
            if (deletedDetailImages.Any())
            {
                FileManager.DeleteAll(_env.WebRootPath,"uploads/ProductImages",deletedDetailImages.Select(x=>x.ImageName).ToList());
            }

            return RedirectToAction("index");
        }

        public void GetPageDetails()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Tags = _context.Tags.ToList();
        }
    }
}
