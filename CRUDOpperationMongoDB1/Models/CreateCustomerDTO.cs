using System.ComponentModel.DataAnnotations;

namespace CRUDOpperationMongoDB1.Models
{
    public class CreateCustomerDTO
    {
       
        public string CustomerName { get; set; }
      
        public string CustomerPhone { get; set; }
 
        public string  Email { get; set; }
    }
}
