using CRUDOpperationMongoDB1.Domain.Entities;

public interface IPostRepository
{
    Task<Post> GetByIdAsync(Guid id);
    Task<Post> GetBySlugAsync(string slug);
    Task<Post> AddAsync(Post post);
    Task<Post> UpdateAsync(Post post);
    Task<List<Post>> SearchAsync(string? keyword);
}

