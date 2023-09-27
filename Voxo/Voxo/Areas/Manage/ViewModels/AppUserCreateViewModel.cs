using System.ComponentModel.DataAnnotations;

namespace Voxo.Areas.Manage.ViewModels
{
    public class AppUserCreateViewModel
    {
        [Required,MaxLength(20)]
        public string UserName { get; set;}
        [Required,MaxLength(50)]
        public string Fullname { get; set;}
        [Required,MaxLength (50)]
        public string Email { get; set;}
        [Required,DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required,DataType(DataType.Password)]

        
        public string Password { get; set;}
        [Required,DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set;}
        [Required]
        public bool IsAdmin { get; set;}
    }
}
