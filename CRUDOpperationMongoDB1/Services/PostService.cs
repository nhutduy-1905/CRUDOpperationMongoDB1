using CRUDOpperationMongoDB1.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CRUDOpperationMongoDB1.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoCollection<Post> _posts;
        public PostService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _posts = database.GetCollection<Post>("Post");
            var indexKeys = Builders<Post>.IndexKeys.Ascending(p => p.Slug);
            _posts.Indexes.CreateOneAsync(new CreateIndexModel<Post>(indexKeys));
            //
        }
  
        //----
        public async Task<Post> CreatePostAsync(CreatePostDTO dto)
        {

            var post = dto.ToEntity();
            post.Slug = GenerateSlug(dto.Title);
            post.Slug = await EnsureUniqueSlug(post.Slug);
            await _posts.InsertOneAsync(post);
            return post;
        }
        public async Task<Post> GetPostBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return null;
            }

            var post = await _posts.Find(p => p.Slug == slug)
                                  .FirstOrDefaultAsync();
            return post;
        }
        private string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            // 
            string slug = RemoveDiacritics(title).ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            slug = Regex.Replace(slug, @"[\s-]+", "-");

            slug = slug.Trim('-');

            return slug;
        }


        // Hàm đảm bảo slug là duy nhất
        private async Task<string> EnsureUniqueSlug(string slug)
        {
            string originalSlug = slug;
            int counter = 1;
            while (await _posts.Find(p => p.Slug == slug).AnyAsync())
            {
                slug = $"{originalSlug}-{counter}";
                counter++;
            }
            return slug;
        }
        public async Task<Post?> UpdatePostAsync(string identifier, CreatePostDTO dto)
        {
            ObjectId objectId;
            Post? existingPost = null;

            // Kiểm tra nếu identifier là ObjectId hợp lệ, tìm theo Id
            if (ObjectId.TryParse(identifier, out objectId))
            {
                existingPost = await _posts.Find(p => p.Id == objectId.ToString()).FirstOrDefaultAsync();

            }
            else
            {
                // Nếu không phải ObjectId, tìm theo Slug
                existingPost = await _posts.Find(p => p.Slug == identifier).FirstOrDefaultAsync();
            }

            if (existingPost == null)
                return null; // Không tìm thấy bài viết

            // Cập nhật các giá trị mới
            var update = Builders<Post>.Update
                .Set(p => p.Title, dto.Title)
                .Set(p => p.Content, dto.Content)
                .Set(p => p.Slug, await EnsureUniqueSlug(GenerateSlug(dto.Title)));

            var result = await _posts.UpdateOneAsync(p => p.Id == existingPost.Id, update);

            if (result.ModifiedCount == 0)
                return null; // Không có thay đổi nào

            return await _posts.Find(p => p.Id == existingPost.Id).FirstOrDefaultAsync();
        }


        public async Task<bool> DeletePostAsync(string slug)
        {
            var result = await _posts.DeleteOneAsync(p => p.Slug == slug);
            return result.DeletedCount > 0;
        }
        private string RemoveDiacritics(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }
        //---
    }
}
// Interface tương ứng
