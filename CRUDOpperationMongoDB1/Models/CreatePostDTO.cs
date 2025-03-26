using System.ComponentModel.DataAnnotations;
namespace CRUDOpperationMongoDB1.Models
{
    public class CreatePostDTO
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
    }
}
