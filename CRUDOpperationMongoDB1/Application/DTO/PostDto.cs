namespace CRUDOpperationMongoDB1.Application.DTO
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
