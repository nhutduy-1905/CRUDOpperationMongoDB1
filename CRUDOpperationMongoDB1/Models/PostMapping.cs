namespace CRUDOpperationMongoDB1.Models
{
    public static class PostMapping
    {
        /// <summary>
        /// Maps a CreatePostDTO to a Post entity.
        /// </summary>
        /// <param name="dto">The CreatePostDTO to map.</param>
        /// <returns>A new Post entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when dto is null.</exception>
        public static Post ToEntity(this CreatePostDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "CreatePostDTO cannot be null.");
            }

            return new Post
            {
                Title = dto.Title ?? string.Empty, // Xử lý null cho Title
                Content = dto.Content // Content có thể null nếu model cho phép
                // Thêm các field khác nếu cần...
            };
        }

        /// <summary>
        /// Maps a Post entity to a CreatePostDTO.
        /// </summary>
        /// <param name="post">The Post entity to map.</param>
        /// <returns>A new CreatePostDTO.</returns>
        /// <exception cref="ArgumentNullException">Thrown when post is null.</exception>
        public static CreatePostDTO ToDto(this Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post), "Post cannot be null.");
            }

            return new CreatePostDTO
            {
                Title = post.Title ?? string.Empty, // Xử lý null cho Title
                Content = post.Content // Content có thể null nếu model cho phép
                // Thêm các field khác nếu cần...
            };
        }
    }
}