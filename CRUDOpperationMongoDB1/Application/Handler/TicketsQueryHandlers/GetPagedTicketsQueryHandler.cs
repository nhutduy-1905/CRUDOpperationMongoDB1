using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Models;

using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Queries.TicketsQueries;
namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    public class GetTicketsPageQueryHandler : IRequestHandler<GetPagedTicketsQuery, PagedResult<Ticket>>
    {
        private readonly ITicketRepository _reponsitory;
      

        public GetTicketsPageQueryHandler(ITicketRepository reponsitory)
        {
            _reponsitory = reponsitory;
        }
        public async Task<PagedResult<Ticket>> Handle(GetPagedTicketsQuery request, CancellationToken cancellationToken)
        {
            return await _reponsitory.GetPageAsync(request.Page, request.PageSize);
        }
    }
}
