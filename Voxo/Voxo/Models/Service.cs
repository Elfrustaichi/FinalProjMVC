using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required,MaxLength(20)]
        public string Header { get; set; }
        [Required,MaxLength (100)]
        public string Text { get; set; }
        [Required,MaxLength(100)]
        public string Icon { get; set; }
        [Required,MaxLength(25)]
        public string Type { get; set; }




    }
}
