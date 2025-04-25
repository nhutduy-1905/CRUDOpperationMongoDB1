using CRUDOpperationMongoDB1.Shared;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    // Command dung de huy 1 ve dua tren id
    public class CancelTicketCommand :IRequest<Result>
    {
        public string TicketId { get; set; }

        public CancelTicketCommand(string ticketId)
        {
            TicketId = ticketId;
        }
    }
}
