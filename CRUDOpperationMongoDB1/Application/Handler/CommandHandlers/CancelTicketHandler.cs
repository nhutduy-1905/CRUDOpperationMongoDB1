using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Enums;
using CRUDOpperationMongoDB1.Shared;
using MediatR;
using CRUDOpperationMongoDB1.Application.DTO;
namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    // handler xu ly logic huy ve
    public class CancelTicketHandler : IRequestHandler<CancelTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;

        public CancelTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<Result> Handle(CancelTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);
            if (ticket == null)
                return Result.Fail("Ve khong ton tai!");
            await _ticketRepository.UpdateStatusAsync(request.TicketId, TicketStatus.Deleted);
            return Result.Ok("Ve da huy thanh cong!", new {TicketId = request.TicketId});
        }
    }
}
