using MediatR;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Shared;


namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                Email = request.Email
            };

            await _customerRepository.CreateCustomerAsync(customer);
            return customer.CustomerId;
        }
    }
}
