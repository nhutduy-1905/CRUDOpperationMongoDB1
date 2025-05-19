using MediatR;
using CRUDOpperationMongoDB1.Shared;

namespace CRUDOpperationMongoDB1.Application.Command.Customer
{
    public class DeleteCustomerCommand : IRequest<Result>
    {
        public string CustomerId { get; set; }
    }
}
