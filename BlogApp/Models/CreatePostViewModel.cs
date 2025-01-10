using BlogApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Title alanı zorunludur.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Content alanı zorunludur.")]
        public string? Content { get; set; }
        [Required(ErrorMessage = "URL alanı zorunludur.")]
        [RegularExpression(@"^[a-z0-9-]+$")]
        public string? Url { get; set; }
        [Required(ErrorMessage = "Description alanı zorunludur.")]
        public string? Description { get; set; }
        [Required]
        public string? Images { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<int> SelectedTagIds { get; set; } = new List<int>();
    }
}
