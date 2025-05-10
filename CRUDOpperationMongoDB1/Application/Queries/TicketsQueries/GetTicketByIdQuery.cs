using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
namespace CRUDOpperationMongoDB1.Application.Queries.TicketsQueries
{
    public class GetTicketByIdQuery : IRequest<Ticket>
    {
        public string Id;
        public GetTicketByIdQuery(string id)
        {
            Id= id;
        }
    }
}
