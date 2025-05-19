using CRUDOpperationMongoDB1.Application.Command.Customer;
using MediatR;
using CRUDOpperationMongoDB1.Shared;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Interfaces;

namespace CRUDOpperationMongoDB1.Application.Handler.CustomerCommandHandlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerRepository.DeleteCustomerAsync(request.CustomerId, cancellationToken);
        }
    }
}
