using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Data;
using CRUDOpperationMongoDB1.Domain.Entities;
using MongoDB.Driver;

namespace CRUDOpperationMongoDB1.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerRepository(IMongoDatabase context)
        {
            _customers= context.GetCollection<Customer>("Customers");
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            await _customers.InsertOneAsync(customer);
        }
        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            return await _customers.Find(x => x.CustomerId == customerId).FirstOrDefaultAsync();
        }
    }
}
