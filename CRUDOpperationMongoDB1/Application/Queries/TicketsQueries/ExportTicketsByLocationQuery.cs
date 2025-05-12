using MediatR;
using Microsoft.AspNetCore.Http;

namespace CRUDOpperationMongoDB1.Application.Queries.TicketsQueries
{
    public class ExportTicketsByLocationQuery : IRequest<byte[]>
    {
        public string? FromAddress { get; set; }
        public string? ToAddress { get; set; }
    }
}
