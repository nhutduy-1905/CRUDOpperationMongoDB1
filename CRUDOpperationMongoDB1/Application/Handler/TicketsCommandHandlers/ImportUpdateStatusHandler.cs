using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Enums;
using CRUDOpperationMongoDB1.Shared;
using CRUDOpperationMongoDB1.Application.DTO;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

// Handler xu ly logic cho mportUpdateStatusCommand
namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    public class ImportUpdateStatusHandler : IRequestHandler<ImportUpdateStatusCommand, Result<string>>
    {
        private readonly ITicketRepository _ticketRepository;

        public ImportUpdateStatusHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<Result<string>> Handle(ImportUpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var updates = request.Updates;
            if (updates == null || updates.Count == 0)
                return Result<string>.Failure("Danh sach cap nhat trong!");

            var invalidTickets = updates.Where(u => !Enum.IsDefined(typeof(TicketType), u.Type)).ToList();
            var invalidStatuses = updates.Where(u => !Enum.IsDefined(typeof(TicketStatus), u.Status)).ToList();

            if (invalidTickets.Any() || invalidStatuses.Any())
            {
                return Result<string>.Failure("Dữ liệu không hợp lệ!");
            }

            var updated = await _ticketRepository.UpdateStatusBulkAsync(updates);

            var resultDto = new
            {
                Message = "Cập nhật thành công!",
                Data = updated
            };

            // Chuyển đối tượng thành JSON
            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(resultDto);
            return Result<string>.Success(resultJson);
        }
    }
}
