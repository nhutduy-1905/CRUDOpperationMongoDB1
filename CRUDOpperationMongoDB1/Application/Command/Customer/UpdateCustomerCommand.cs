using System.Reflection.Metadata;
using CRUDOpperationMongoDB1.Application.DTO;
using MediatR;
namespace CRUDOpperationMongoDB1.Application.Command.Customer
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
    }
}
