using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CRUDOpperationMongoDB1.Models
{
    [BsonCollection("booking_log")]
    [BsonIgnoreExtraElements]
    public class Items
    {
    
    [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public double Price { get; set; }

    }
}
