using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class ContactUsRequest
    {
        public int Id { get; set; }
        [Required,MaxLength(25)]
        public string Name { get; set; }
        [Required,MaxLength(25)]
        public string Surname { get; set; }
        [Required,MaxLength (50)]
        public string Email { get; set; }
        [Required,MaxLength(500)]
        public string RequestText { get; set; }
        [MaxLength(500)]
        public string ReplyText { get; set; }
    }
}
