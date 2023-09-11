using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class Setting
    {
        [MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
    }
}
