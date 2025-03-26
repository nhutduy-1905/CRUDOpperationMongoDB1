using CRUDOpperationMongoDB1.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CRUDOpperationMongoDB1.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;
        public PostService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _posts = database.GetCollection<Post>("Post");
        }
        public async Task<Post> CreatePostAsync(CreatePostDTO dto)
        {
            var post = dto.ToEntity();
            await _posts.InsertOneAsync(post);
            return post;
        }
        public async Task<Post> GetPostBySlugAsync(string slug)
        {
            return await _posts.Find(p => p.Slug == slug).FirstOrDefaultAsync();
        }
        public async Task<Post> UpdatePostAsync(string id, CreatePostDTO dto)
        {
            var existingPost = await _posts.Find(p => p.Id == id).FirstOrDefaultAsync();
            if(existingPost == null)
            {
                return null;
            }
            var updatedPost = dto.ToEntity();
            updatedPost.Id = existingPost.Id;
            var result = await _posts.ReplaceOneAsync(p => p.Id == id, updatedPost);
            return result.ModifiedCount > 0 ? updatedPost : null;
        }
        public async Task<bool> DeletePostAsync(string id)
        {
            var result = await _posts.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
