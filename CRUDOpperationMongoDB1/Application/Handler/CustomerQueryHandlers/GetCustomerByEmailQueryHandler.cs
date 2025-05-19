using AutoMapper;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Mapper;
using CRUDOpperationMongoDB1.Application.Queries.CustomerQueries;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Shared;
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
            // nếu thất bại  ném ex
            Result<Domain.Entities.Customer> repoResult = await _customerRepository.GetCustomerByEmailAsync(request.Email, cancellationToken);
            if (!repoResult.IsSuccess) throw new KeyNotFoundException(repoResult.ErrorMessage);
            // thanh công
            var customerEntity = repoResult.Data;
            return CustomerMapper.ToDto(customerEntity);
         
           
        }
        // hàm kiểm tra email
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
