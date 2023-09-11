using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class UserCartItem
    {
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }

        public string AppUserId { get; set; }

        public int ProductId { get; set; }

        public AppUser AppUser { get; set; }

        public Product Product { get; set; }
    }
}
