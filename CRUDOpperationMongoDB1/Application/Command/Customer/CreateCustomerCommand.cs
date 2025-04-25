using CRUDOpperationMongoDB1.Shared;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Command.Customer
{
    public class CreateCustomerCommand : IRequest<Result>
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        
        public string Email { get; set; }
    }
}
