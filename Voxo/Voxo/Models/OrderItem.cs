using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voxo.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitCostPrice { get; set; }
        [Range(0,100)]
        public int DiscountPercent { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
