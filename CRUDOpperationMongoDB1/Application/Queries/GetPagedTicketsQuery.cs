using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Models;
namespace CRUDOpperationMongoDB1.Application.Queries
{
    public class GetPagedTicketsQuery : IRequest<PagedResult<Ticket>>
    {
        public int Page { get; }
        public int PageSize { get; }

        public GetPagedTicketsQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

    }
}
