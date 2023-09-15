using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string ImageName { get; set; }

        public bool PosterStatus { get; set; }

        public int ProductId { get; set; }

        public bool DetailPicture { get; set; }

        public Product Product { get; set; }
    }
}
