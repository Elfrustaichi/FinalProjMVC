using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class PaymentCard
    {
        public int Id { get; set; }
        [Required,MaxLength(20)]
        public string NameOnCard { get; set; }
        [Required,MaxLength(16)]
        public string CardNumber { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [Required,MaxLength(3)]
        public string CVC { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
