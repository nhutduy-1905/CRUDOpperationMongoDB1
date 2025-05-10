using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Domain.Entities;

namespace CRUDOpperationMongoDB1.Application.Mapper
{
    public class CustomerMapper
    {
        public static CustomerDto ToDto (Customer customer)
        {
            if (customer == null) return null;
            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                Email = customer.Email,
            };
        }
        public static Customer ToEntity(CustomerDto customerDto)
        {
            return new Customer
            {
                CustomerId = customerDto.CustomerId,
                CustomerName = customerDto.CustomerName,
                CustomerPhone = customerDto.CustomerPhone,
                Email = customerDto.Email,
            };
        }
    }
}
