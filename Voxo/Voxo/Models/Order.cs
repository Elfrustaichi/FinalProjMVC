using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voxo.Enums;

namespace Voxo.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string Fullname { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Required,MaxLength (50)]
        public string Country { get; set; }
        [Required,MaxLength(50)]
        public string State { get; set; }
        [Required,MaxLength(20)]
        public string PostalCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string Note { get; set; }

        public DateTime CreateTime { get; set; }
        [Required]
        public OrderStatus Status { get; set; }

        public int AdressId { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public Adress Adress { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
