using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        [Required,MaxLength(500)]
        public string ReviewText { get; set; }
        [Required,Range(0,5)]
        public int Rate { get; set; }

        public DateTime CreateTime { get; set; }

        public int ProductId { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public Product Product { get; set; }
        
    }
}
