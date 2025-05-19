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
        private readonly ILogger<GetCustomerByIdQueryHandler> _logger;
        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, ILogger<GetCustomerByIdQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }
        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
           var result = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);
            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Customer not found or retrieval failed.");
                throw new InvalidOperationException("Customer not found.");
            }
            var customer = result.Data;
            var customerDto = CustomerMapper.ToDto(customer);
            return customerDto;

        }
    }
}
