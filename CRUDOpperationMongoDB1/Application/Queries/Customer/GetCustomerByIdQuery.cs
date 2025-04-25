using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;

namespace CRUDOpperationMongoDB1.Application.Queries.Customers
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public string CustomerId { get; set; }

        public GetCustomerByIdQuery(string customerId)
        {
            CustomerId = customerId;
        }
    }
}