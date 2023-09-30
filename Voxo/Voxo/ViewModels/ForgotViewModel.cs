using System.ComponentModel.DataAnnotations;

namespace Voxo.ViewModels
{
    public class ForgotViewModel
    {
        [Required,MaxLength(50),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
