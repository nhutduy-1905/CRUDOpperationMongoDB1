using MediatR;
using System.Reflection.Metadata;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.DTO;

namespace CRUDOpperationMongoDB1.Application.Queries.CustomerQueries
{
    public record GetCustomerByEmailQuery :IRequest<CustomerDto>
    {
        public string Email { get; set; }
        public GetCustomerByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
