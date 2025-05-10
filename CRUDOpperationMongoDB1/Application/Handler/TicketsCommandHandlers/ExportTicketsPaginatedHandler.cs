using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Queries;
using MediatR;
using OfficeOpenXml;

namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    public class ExportTicketsPaginatedHandler : IRequestHandler<ExportTicketsPaginatedQuery, byte[]>
    {
        private readonly ITicketRepository _ticketRepository;
        public ExportTicketsPaginatedHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<byte[]> Handle(ExportTicketsPaginatedQuery request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var tickets = await _ticketRepository.FindTickets(_ => true);
            var paginated = tickets.Skip((request.Page -1) * request.PageSize).Take(request.PageSize).ToList();
            using var package = ExcelHelper.GenerateExcel(paginated);
            return package.GetAsByteArray();
        }
    }
}
