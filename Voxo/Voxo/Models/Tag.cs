using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required,MaxLength(20)]
        public string Name { get; set; }

        public List<ProductTag> ProductTags { get; set; }
    }
}
