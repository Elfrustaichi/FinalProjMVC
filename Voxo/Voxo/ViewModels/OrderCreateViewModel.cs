using System.ComponentModel.DataAnnotations;

namespace Voxo.ViewModels
{
    public class OrderCreateViewModel
    {
        [ MaxLength(50)]
        public string Fullname { get; set; }
        [ MaxLength(50)]
        public string FullAddress { get; set; }
        [ MaxLength(20)]
        public string PostalCode { get; set; }
        [ MaxLength(50)]
        public string Email { get; set; }
        [ MaxLength(255)]
        public string Note { get; set; }
        
    }
}
