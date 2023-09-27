using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voxo.Attributes.ValidationAttributes;

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
        public bool IsNewArrival { get; set;}
        [Required]
        public bool StockStatus { get; set; }

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Brand Brand { get; set; }

        public Slider Slider { get; set; }

        public List<ProductTag> ProductTags { get; set; }=new List<ProductTag>();

        public List<ProductImage> ProductImages { get; set; }= new List<ProductImage>();

        public List<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public List<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

        public List<UserWishlistItem> UserWishListItems { get; set; } = new List<UserWishlistItem>();

        public List<UserCartItem> UserCartItemListItems { get; set;}=new List<UserCartItem>();

        [NotMapped]
        [MaxFileSizeValidation(10485760)]
        [RequiredFileTypeValidation("image/png", "image/jpeg", "image/jpg")]
        public IFormFile PosterImage { get; set; }
        [NotMapped]
        [MaxFileSizeValidation(10485760)]
        [RequiredFileTypeValidation("image/png", "image/jpeg", "image/jpg")]
        public IFormFile HoverImage { get; set; }
        [NotMapped]
        [MaxFileSizeValidation(10485760)]
        [RequiredFileTypeValidation("image/png", "image/jpeg", "image/jpg")]
        public List<IFormFile> DetailImages { get; set; }=new List<IFormFile>();
        [NotMapped]
        [Required]
        public List<int> SizeIds { get; set; }=new List<int>();
        [NotMapped]
        [Required]
        public List<int> TagIds { get; set; }=new List<int>();
        [NotMapped]
        public List<int> DetailImageIds { get;set; }=new List<int>();



    }
}
