using CRUDOpperationMongoDB1.Models;

namespace CRUDOpperationMongoDB1.Services
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(CreatePostDTO postDto);
        Task<Post> GetPostBySlugAsync(string slug);
        Task<Post> UpdatePostAsync(string slug, CreatePostDTO postDto); // Sử dụng slug thay vì id
        Task<bool> DeletePostAsync(string slug); // Sử dụng slug thay vì id
    }
}
