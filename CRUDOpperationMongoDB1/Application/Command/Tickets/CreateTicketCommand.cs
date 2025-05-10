using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Domain.Enums;
using CRUDOpperationMongoDB1.Application.DTO;

namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    public class CreateTicketCommand : IRequest<TicketDto>
    {
        public string  TicketType { get; set; }
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