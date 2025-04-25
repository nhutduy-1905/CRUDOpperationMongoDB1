using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Enums;
using CRUDOpperationMongoDB1.Shared;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;

// Handler xu ly logic cho mportUpdateStatusCommand
namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    public class ImportUpdateStatusHandler : IRequestHandler<ImportUpdateStatusCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;

        public ImportUpdateStatusHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<Result> Handle(ImportUpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var updates = request.Updates;
            if (updates == null || updates.Count == 0)
                return Result.Fail("Danh sach cap nhat trong!");

            var invalidTickets = updates.Where(u => !Enum.IsDefined(typeof(TicketType), u.Type)).ToList();
            var invalidStatuses = updates.Where(u => !Enum.IsDefined(typeof(TicketStatus), u.Status)).ToList();

            if (invalidTickets.Any() || invalidStatuses.Any())
            {
                return Result.Fail("Du lieu khong hop le!", new { invalidTickets, invalidStatuses });
            }
            var updated = await _ticketRepository.UpdateStatusBulkAsync(updates);
            return Result.Ok("Cap nhat thanh cong!", updated);
        }
    }
}
