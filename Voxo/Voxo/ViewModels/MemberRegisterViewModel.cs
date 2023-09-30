using System.ComponentModel.DataAnnotations;

namespace Voxo.ViewModels
{
    public class MemberRegisterViewModel
    {
        [Required,MaxLength(20)]
        public string Username { get; set; }
        [Required,MaxLength(50),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,DataType(DataType.Password),MinLength(8)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
