using AutoMapper;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Mapper;
using CRUDOpperationMongoDB1.Application.Queries.CustomerQueries;
using CRUDOpperationMongoDB1.Domain.Entities;
using MediatR;
using System.Text.RegularExpressions;

namespace CRUDOpperationMongoDB1.Application.Handler.CustomerQueryHandlers
{
    public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByEmailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            // kiểm tra định dạng email dùng regex đảm bảo email đúng
            if (!IsValidEmail(request.Email))
            {
                throw new AggregateException("Email không hợp lệ.");
            }
            // tìm khách hàng theo email
            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
            //nếu null quăng ra lỗi
            if (customer == null)
            {
                throw new Exception("Không tìm thấy khách hàng.");
            }
            // ánh xạ từ entity sang dto
            return CustomerMapper.ToDto(customer);
        }
        // hàm kiểm tra email
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
