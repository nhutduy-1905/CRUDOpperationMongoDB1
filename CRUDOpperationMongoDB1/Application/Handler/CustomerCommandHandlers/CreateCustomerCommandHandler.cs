using MediatR;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Shared;
using CRUDOpperationMongoDB1.Application.Mapper;
using CRUDOpperationMongoDB1.Application.DTO;

namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository; 
        }
        // Handle : xử lý command CreateCustomerCommand được gửi đến qua MediaR nhận thông tin request
        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // tạo đối tượng khách hàng mới từ thông tin nhận được chuyển từ dto --> entity
            var customer = CustomerMapper.ToEntity(request);
            // Gửi yêu cầu tạo customer
            var result = await _customerRepository.CreateCustomerAsync(customer, cancellationToken);
            // Thành công --> chuyển sang DTO trả về
            if (result.IsSuccess)
                return CustomerMapper.ToDto(result.Data);
            // Thất bại ném lỗi
            throw new ApplicationException(result.ErrorMessage);
           

        }
    }
}
