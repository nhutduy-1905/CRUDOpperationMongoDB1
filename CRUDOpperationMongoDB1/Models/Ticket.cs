using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketAPI.Models
{


    [BsonIgnoreExtraElements]
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("TicketType")]
        public string TicketType { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        [BsonElement("FromDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FromDate { get; set; }

        [BsonElement("ToDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime ToDate { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("CustomerName")]
        public string CustomerName { get; set; }

        [BsonElement("CustomerPhone")]
        public string CustomerPhone { get; set; }


        public string Status { get; set; } = "Hoạt động"; // Mặc định là hoạt động khi tạo vé mới   
    }   

}
