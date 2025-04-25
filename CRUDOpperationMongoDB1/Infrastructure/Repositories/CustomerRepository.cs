using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Data;
using CRUDOpperationMongoDB1.Domain.Entities;
using MongoDB.Driver;

namespace CRUDOpperationMongoDB1.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IApplicationDbContext _context;

        public CustomerRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.InsertOneAsync(customer);
        }
        public async Task<Customer> GetByIdAsync(string customerId)
        {
            return await _context.Customers.Find(c => c.CustomerId == customerId).FirstOrDefaultAsync();
        }
    }
}
