using CRUDOpperationMongoDB1.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRUDOpperationMongoDB1.Application.Queries.TicketsQueries
{
    public class ExportTicketsQuery :IRequest<IActionResult>
    {
        public TicketDto Filter {  get; set; }

        public ExportTicketsQuery() { }
        public ExportTicketsQuery(TicketDto filter)
        {
            Filter = filter;
        }
    }
}
