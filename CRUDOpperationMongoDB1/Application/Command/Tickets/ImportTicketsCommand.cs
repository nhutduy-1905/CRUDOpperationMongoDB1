using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    public class ImportTicketsCommand :IRequest<IActionResult>
    {
        public IFormFile File { get; set; }
    }
}
