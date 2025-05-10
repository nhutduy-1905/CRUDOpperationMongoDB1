using MediatR;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Queries.Customers;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.Mapper;


namespace CRUDOpperationMongoDB1.Application.Handler.CustomerQueryHandlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customerId = await _customerRepository.GetCustomerByIdAsync(request.CustomerId);
            if (customerId == null) return null;

            var customerDto = CustomerMapper.ToDto(customerId);
            return customerDto;

        }
    }
}
