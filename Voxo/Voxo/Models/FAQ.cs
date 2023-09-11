

using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        [Required,MaxLength(500)]
        public string QuestionText { get; set; }
        [Required,MaxLength(1000)]
        public string AnswerText { get; set; }
    }
}
