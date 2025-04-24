using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Domain.Enums;
using MediatR;
namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    public class SearchTicketsQuery : IRequest<List<TicketDto>>
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime? FromDate { get; set; } // 👈 phải có dấu hỏi vi: HasValue : dung cho null valiable
        public DateTime? ToDate { get; set; }
    }
}
