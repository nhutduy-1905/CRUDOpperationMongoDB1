using TicketAPI.Models;

namespace CRUDOpperationMongoDB1.Services
{
    public interface ICustomerSerVice
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(string customerId);
        Task<Customer> GetCustomerByEmailAsync(string email);
    }
}
