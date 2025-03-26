
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TicketAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("CustomerName")]
        public string CustomerName { get; set; }
        [BsonElement("CustomerPhone")]
        public string CustomerPhone { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }


    }
}

