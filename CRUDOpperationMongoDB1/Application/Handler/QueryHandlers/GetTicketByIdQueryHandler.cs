using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CRUDOpperationMongoDB1.Application.Queries;

namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, Ticket>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicketByIdQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<Ticket> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            return await _ticketRepository.GetTicketByIdAsync(request.Id);
        }
    }

}
