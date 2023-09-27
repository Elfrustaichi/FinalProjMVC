using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class Adress
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Fullname { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullAdress { get; set; }
        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public List<Order> Orders { get; set; }= new List<Order>();
    }
}
