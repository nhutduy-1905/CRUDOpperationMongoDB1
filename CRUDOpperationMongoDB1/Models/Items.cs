using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CRUDOpperationMongoDB1.Models
{

    [BsonIgnoreExtraElements]
    // Test
    public class Ticket
    {
        public int Id { get; set; }
        public string TicketType { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
    public class TicketDTO
    {
        public string TicketType { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string TravelDateRange { get; set; } 
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
}
    

        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //// Test

//        // PRESENT
//        public ObjectId Id { get; set; }

//        [BsonElement("Name")]
//        public string Name { get; set; }

//        //public double Price { get; set; }

//        [BsonElement("Price")]
//        public double Price { get; set; }

//        [BsonElement("Description")]
//        public string Description { get; set; }

//        [BsonElement("Brand")]
//        public string Brand { get; set; }

//        [BsonElement("Color")]
//        public string Color { get; set; }

//    }
//    public class ItemDTO
//    {
//        public ObjectId Id { get; set; }
//        public string Name { get; set; }
//        public double Price { get; set; }
//        public string Description { get; set; }
//    }
//    public class CreateItemDTO
//    {

//        public string Name { get; set; }
//        public double Price { get; set; }
//        public string Description { get; set; }
//        public string Brand { get; set; }
//        public string Color { get; set; }
//    }

//}
