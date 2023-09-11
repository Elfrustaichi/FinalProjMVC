using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voxo.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required,Column(TypeName ="money")]
        public decimal SalePrice { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        [Range(0,100)]
        public int DiscountPercent { get; set; }
        [Range(0,5)]
        public int Rate { get; set; }
        [Required]
        public bool StockStatus { get; set; }

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Brand Brand { get; set; }

        public Slider Slider { get; set; }

        public List<ProductTag> ProductTags { get; set; }

        public List<ProductImage> ProductImages { get; set; }

        public List<ProductSize> ProductSizes { get; set; }   
        
        public List<OrderItem> OrderItems { get; set; }

        public List<ProductReview> ProductReviews { get; set; }

        public List<UserWishlistItem> UserWishListItems { get; set;}

        public List<UserCartItem> UserCartItemListItems { get; set;}

       
    }
}
