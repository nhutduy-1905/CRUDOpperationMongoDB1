
namespace CRUDOpperationMongoDB1.Domain.Entities
{
    public class Customer
    {
        public string CustomerId { get; set; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Email { get; set; }
    }
}
