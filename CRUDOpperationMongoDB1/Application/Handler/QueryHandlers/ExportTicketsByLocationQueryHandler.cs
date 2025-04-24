using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Queries;
using CRUDOpperationMongoDB1.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using OfficeOpenXml;

namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    public class ExportTicketsByLocationQueryHandler : IRequestHandler<ExportTicketsByLocationQuery, byte[]>
    {
        private readonly ITicketRepository _ticketRepository;

        public ExportTicketsByLocationQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
      
        public async Task<byte[]> Handle(ExportTicketsByLocationQuery request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var filter = Builders<Ticket>.Filter.Empty;
            if (!string.IsNullOrEmpty(request.FromAddress) && !string.IsNullOrEmpty(request.ToAddress))
                filter = Builders<Ticket>.Filter.And(
                    Builders<Ticket>.Filter.Eq(t => t.FromAddress, request.FromAddress),
                    Builders<Ticket>.Filter.Eq(t => t.ToAddress, request.ToAddress));
            else if (!string.IsNullOrEmpty(request.FromAddress))
                filter = Builders<Ticket>.Filter.Eq(t => t.FromAddress, request.FromAddress);
            else if (!string.IsNullOrEmpty(request.ToAddress))
                filter = Builders<Ticket>.Filter.Eq(t => t.ToAddress, request.ToAddress);
            var tickets = await _ticketRepository.FindTickets(filter);
            if (!tickets.Any()) throw new InvalidOperationException("Khong co du lieu de xuat.");
            using var package = ExcelHelper.GenerateExcel(tickets);
            return package.GetAsByteArray();
        }
    }
}
