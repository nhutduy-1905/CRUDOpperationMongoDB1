using CRUDOpperationMongoDB1.Shared;
// dung thư viện shared  chứa các kiểu dữ liệu dùng chung:
// Lớp result<T> để bọc kết quả trả về {success, false, thông báo, dữ liệu}
using MediatR;
// gửi đến handler thực thu
namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    // Command dung để hủy vé
    // khi command đi sẽ nhận về object Result<string>
    public class CancelTicketCommand :IRequest<Result<string>>
    {
        public string TicketId { get; set; }

        // controctor giúp khởi tạo command với ticketId
        public CancelTicketCommand(string ticketId)
        {
            // Phải cung cấp id khi tạo command
            TicketId = ticketId;
        }
    }
}
