using MediatR;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Shared;


namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var newCustomer = new Customer
            {
                CustomerId = Guid.NewGuid().ToString(), // tự tạo ID
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                Email = request.Email
            };

            await _customerRepository.AddAsync(newCustomer);
            return Result.Ok("Thêm thành công!"); // ✅ Tạo đối tượng Result thành công
        }
    }
}
