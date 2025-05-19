using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Shared;
using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.Mapper;

namespace CRUDOpperationMongoDB1.Application.Handler.CustomerCommandHandlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                Email = request.CustomerEmail,
                
            };
            var result = await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);
            if (result == null)
                throw new ApplicationException("Repository trả về null.");
            if (!result.IsSuccess)
                throw new ApplicationException(result.ErrorMessage?? "Cập nhật thất bại");
            return CustomerMapper.ToDto(customer);
        }

    }
}
