using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    // nhập vé từ 1 file
    // khi commmand này gửi đến Media --> nhận lại IActionResult
    public class ImportTicketsCommand :IRequest<IActionResult>
    {
        // ASP.net làm việc với file: truy cập tên, nội dung
        public IFormFile File { get; set; }
    }
}
