using Voxo.Models;

namespace Voxo.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }

        public List<Product> RelatedProducts { get; set; }

        public ProductReview ProductReview { get; set; }
    }
}
