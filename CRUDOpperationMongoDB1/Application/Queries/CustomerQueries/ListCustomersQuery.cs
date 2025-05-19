using CRUDOpperationMongoDB1.Models;
using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
namespace CRUDOpperationMongoDB1.Application.Queries.CustomerQueries
{
    public class ListCustomersQuery :IRequest<PagedResult<Customer>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
