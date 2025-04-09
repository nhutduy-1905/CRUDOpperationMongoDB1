using CRUDOpperationMongoDB1.Models;

namespace CRUDOpperationMongoDB1.Services
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(CreatePostDTO postDto);
        Task<Post> GetPostBySlugAsync(string slug);
        Task<Post> UpdatePostAsync(string id, CreatePostDTO postDto); 
        Task<bool> DeletePostAsync(string id);
        Task<List<Post>> ExportPost();
        Task<List<Post>> ExportByFilter(CreatePostDTO postDto);
    }
}
