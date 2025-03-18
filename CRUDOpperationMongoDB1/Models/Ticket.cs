using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TicketAPI.Models
{
    public enum TicketStatus
    {
        Active = 0,
        Inactive = 1,
        Deleted = 2
    }
    public enum TicketType
    {
        KhuHoi,
        MotChieu
    }

    [BsonIgnoreExtraElements]
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]  // ✅ Lưu Enum dưới dạng string thay vì số
        public TicketType TicketType { get; set; }

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


        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public TicketStatus  Status { get; set; } // Mặc định là "Active" khi tạo vé mới   
    }
}
