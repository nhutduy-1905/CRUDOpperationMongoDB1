using MongoDB.Driver;
using TicketAPI.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using CRUDOpperationMongoDB1.Models;
using MongoDB.Bson;
using CRUDOpperationMongoDB1.Services;
namespace TicketAPI.Service
{
    public class CustomerService :ICustomerSerVice
    {
        private readonly IMongoCollection<Customer> _customers;
        public CustomerService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _customers = database.GetCollection<Customer>("Customers");
        }
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customers.Find(_ => true).ToListAsync();
        }
        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return null;
            }

            try
            {
                return await _customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                Console.WriteLine($"Lỗi khi gọi GetCustomerByIdAsync: {ex.GetType().Name} - {ex.Message}\nStackTrace: {ex.StackTrace}");
                return null; // Trả về null thay vì ném ngoại lệ
            }
        }
        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _customers.Find(c => c.Email == email).FirstOrDefaultAsync();
        }
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
             await _customers.InsertOneAsync(customer);
             return customer;           
        }
    }
}
