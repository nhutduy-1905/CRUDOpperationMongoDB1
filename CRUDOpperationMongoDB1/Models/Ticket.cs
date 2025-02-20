using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketAPI.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // Chuyển đổi từ string sang ObjectId tự động
        public string Id { get; set; }

        public string TicketType { get; set; } // "Khứ hồi", "Một chiều"
        public string FromAddress { get; set; } // "HCM"
        public string ToAddress { get; set; } // "HN"

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FromDate { get; set; } // "21/01/2025"

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime ToDate { get; set; } // "10/02/2025"

        public int Quantity { get; set; } // 1
        public string CustomerName { get; set; } // "Nguyễn Văn A"
        public string CustomerPhone { get; set; } // "0988989890"
    }
}
