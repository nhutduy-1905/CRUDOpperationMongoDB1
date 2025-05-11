using CRUDOpperationMongoDB1.Shared;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Command.Customer
{
    // CreateCustomerCommand:request gửi đến MediaR yêu cầu tạo một class Customer
    // khi request xử lý nó sẽ trả về kiểu string
    public class CreateCustomerCommand : IRequest<string>
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        
        public string Email { get; set; }
    }
}
