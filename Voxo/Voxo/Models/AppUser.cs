using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class AppUser:IdentityUser
    {
        [MaxLength(50)]
        public string Fullname { get; set; }
        [Required] 
        public bool IsAdmin { get; set; }
        [MaxLength(50)]
        public string Adress { get; set; }
        public string ConnectionId { get; set; }

        public DateTime CreationTime { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        public List<UserWishlistItem> WishlistItems { get; set; }=new List<UserWishlistItem>();

        public List<UserCartItem> CartItems { get; set; } = new List<UserCartItem>();

        public List<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
    }
}
