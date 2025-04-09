using System.ComponentModel.DataAnnotations;
namespace CRUDOpperationMongoDB1.Models
{
    public class CreatePostDTO
    {
        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
