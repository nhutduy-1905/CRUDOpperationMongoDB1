using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CRUDOpperationMongoDB1.Domain.Entities
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } // khoa chinh
        public string Slug { get; set; } // duong dan 
        public string Title { get; set; } // tieu de
        public string Content { get; set; } // noi dung bai viet 
        public string  Author { get; set; } // ten tac gia
        public DateTime CreatedAt  { get; set; } // ngay tao
        public DateTime? UpdatedAt { get; set; } // ngay cap nhat

    }
}
