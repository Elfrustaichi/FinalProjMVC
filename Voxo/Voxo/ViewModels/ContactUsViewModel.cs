using System.ComponentModel.DataAnnotations;

namespace Voxo.ViewModels
{
    public class ContactUsViewModel
    { 
        [Required,MaxLength(25)]
        public string Name { get; set; }
        [Required, MaxLength(25)]
        public string Surname { get; set; }
        [Required, MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MaxLength(50)]
        public string Subject { get; set; }
        [Required, MaxLength(500)]
        public string Comment { get; set; }
    }
}
