using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.DTO;
namespace CRUDOpperationMongoDB1.Application.Queries.Customers
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public string CustomerId { get; set; }
    }
}