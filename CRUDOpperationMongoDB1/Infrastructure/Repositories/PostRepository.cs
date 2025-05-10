using CRUDOpperationMongoDB1.Data;
using CRUDOpperationMongoDB1.Domain.Entities;
using MongoDB.Driver;

namespace CRUDOpperationMongoDB1.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _posts;

        public PostRepository(IApplicationDbContext context)
        {
            _posts = context.Post;  // lấy collection Post từ context
        }

        public async Task<Post> AddAsync(Post post)
        {
            await _posts.InsertOneAsync(post);
            return post;
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, id);
            return await _posts.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Post> GetBySlugAsync(string slug)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Slug, slug);
            return await _posts.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, post.Id);
            var result = await _posts.ReplaceOneAsync(filter, post);
            if(result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return post;
            }
            return null;
        }

        public async Task<List<Post>> SearchAsync(string? keyword, int page, int pageSize)
        {
            var filter = Builders<Post>.Filter.Empty;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = Builders<Post>.Filter.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            }

            return await _posts.Find(filter)
                .SortByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }
    }
}
