using MediatR;
using CRUDOpperationMongoDB1.Application.Queries.Customers;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Application.Interfaces;

namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByIdQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new Exception("Không tìm thấy khách hàng");
            return customer;
        }
    }
}