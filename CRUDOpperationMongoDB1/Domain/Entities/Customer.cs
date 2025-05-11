
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CRUDOpperationMongoDB1.Domain.Entities
{
    [BsonIgnoreExtraElements]  // bỏ qua các trường không khai báo trong class
    public class Customer
    {
        [BsonId]  // Khai báo đây là trường _id
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } = ObjectId.GenerateNewId().ToString();
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Email { get; set; }
    }
}
