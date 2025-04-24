using CRUDOpperationMongoDB1.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CRUDOpperationMongoDB1.Domain.Entities
{
    public class Ticket
    {
        [BsonId] // Đánh dấu đây là ID chính
        [BsonRepresentation(BsonType.ObjectId)] // Cho phép Mongo sử dụng ObjectId dạng string
        public string Id { get; set; }
        [BsonRepresentation(BsonType.String)] // convert de chuyen so sang string 
        public TicketType TicketType { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        [BsonRepresentation(BsonType.String)]
        public TicketStatus Status { get; set; }
    }
}