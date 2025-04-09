using TicketAPI.DTOs;
using TicketAPI.Models;

namespace CRUDOpperationMongoDB1.Models
{
    public  static class CustomerMapping 
    {
        public static CreateCustomerDTO ToDto(this Customer customer)
        {
         
            if (customer == null) return null;

            return new CreateCustomerDTO
            {
                
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                Email = customer.Email
            };
        }

        // Ánh xạ từ CustomerDto sang Customer (entity)
        public static Customer ToEntity(this CreateCustomerDTO customerDto)
        {
            if (customerDto == null) return null;

            return new Customer
            {
          
                CustomerName = customerDto.CustomerName,
                CustomerPhone = customerDto.CustomerPhone,
                Email = customerDto.Email
            };
        }
    }
}
