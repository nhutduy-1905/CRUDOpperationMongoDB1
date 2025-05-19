using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Queries.CustomerQueries;
using CRUDOpperationMongoDB1.Models;
using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
namespace CRUDOpperationMongoDB1.Application.Handler.CustomerQueryHandlers
{
    public class ListCustomersQueryHandler :IRequestHandler<ListCustomersQuery, PagedResult<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        public ListCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<PagedResult<Customer>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
        {
           return await _customerRepository.ListCustomersAsync(request.Page, request.PageSize, cancellationToken);
        }
    }
}
