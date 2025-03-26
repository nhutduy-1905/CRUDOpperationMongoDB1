using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CRUDOpperationMongoDB1.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Title")]
        public string Title { get; set; }
        [BsonElement("Slug")]
        public string Slug { get; set; }

        [BsonElement("Content")]
        public string Content { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
