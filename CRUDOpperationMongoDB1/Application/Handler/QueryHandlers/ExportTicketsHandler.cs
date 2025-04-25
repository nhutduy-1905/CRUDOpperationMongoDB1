using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Queries;
using CRUDOpperationMongoDB1.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    public class ExportTicketsQueryHandler : IRequestHandler<ExportTicketsQuery, IActionResult>
    {
        // Inject ITicketRepository into the class
        private readonly ITicketRepository _ticketRepository;

        public ExportTicketsQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository; // Assign injected repository to the field
        }

        public async Task<IActionResult> Handle(ExportTicketsQuery request, CancellationToken cancellationToken)
        {
            // Lấy danh sách vé theo bộ lọc từ request.Filter
            var filtered = await _ticketRepository.GetTicketsByFilterAsync(request.Filter);

            // Tạo danh sách dữ liệu Excel (ở đây chỉ giả lập trả về dữ liệu, bạn có thể tích hợp thư viện xuất Excel như EPPlus, ClosedXML sau)
            var result = new List<object>();

            for (int i = 0; i < filtered.Count; i++)
            {
                var ticket = filtered[i];

                result.Add(new
                {
                    STT = i + 1,
                    ticket.Id,
                    ticket.TicketType,
                    ticket.FromAddress,
                    ticket.ToAddress,
                    ticket.FromDate,
                    ticket.ToDate,
                    ticket.Quantity,
                    ticket.CustomerName,
                    ticket.CustomerPhone,
                    ticket.Status
                });
            }

            // Trả về dữ liệu dưới dạng JSON (có thể đổi thành file Excel sau)
            return new OkObjectResult(result);
        }
    }
}
