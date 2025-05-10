using CRUDOpperationMongoDB1.Domain.Entities;

namespace CRUDOpperationMongoDB1.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(string customerId);
 
    }
}
