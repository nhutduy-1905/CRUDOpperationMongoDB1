using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Domain.Enums;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    public class UpdateTicketCommand : IRequest<Ticket>
    {
        public string Id { get; set; } // Id để xác định ticket cần cập nhật (dùng string vì bạn dùng MongoDB)
        public string TicketType { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; }
    }

}

