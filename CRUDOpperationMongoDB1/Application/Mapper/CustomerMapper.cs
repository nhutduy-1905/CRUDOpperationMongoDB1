using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Domain.Entities;

namespace CRUDOpperationMongoDB1.Application.Mapper
{
    public class CustomerMapper
    {
     
        public static Customer ToEntity(CreateCustomerCommand command)
        {
            return new Customer
            {
                CustomerName = command.CustomerName,
                CustomerPhone = command.CustomerPhone,
                Email = command.Email,
            };
        }
        public static CustomerDto ToDto(Customer customer)
        {
            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                Email = customer.Email,
            };
        }
    }
}
