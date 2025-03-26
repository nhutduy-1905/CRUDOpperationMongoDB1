namespace CRUDOpperationMongoDB1.Models
{
    public  static class PostMapping
    {
       public static Post ToEntity(this CreatePostDTO dto)
        {
            return new Post
            {
                Title = dto.Title,
                Slug = dto.Slug,
                Content = dto.Content
            };
        }
        public static CreatePostDTO toDto(this Post post)
        {
            return new CreatePostDTO
            {
                Title = post.Title,
                Slug = post.Slug,
                Content = post.Content
            };
        }
    }
}
