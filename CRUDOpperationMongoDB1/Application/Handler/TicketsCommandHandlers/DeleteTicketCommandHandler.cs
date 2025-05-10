using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Application.Interfaces;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    public class DeleteTicketCommandHandler :IRequestHandler<DeleteTicketCommand, bool>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger<DeleteTicketCommandHandler> _logger;
        public DeleteTicketCommandHandler(ITicketRepository ticketRepository, ILogger<DeleteTicketCommandHandler> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            // Ghi log khi xoa ticket 
            _logger.LogInformation($"Attempting to delete ticket with Id; {request.Id}");
            var result = await _ticketRepository.DeleteTicketAsync(request.Id);
            if (result)
            {
                // xoa thanh cong
                _logger.LogInformation($"Sucessfully deleted ticked with Id: {request.Id}");
            }
            else
            {
                _logger.LogWarning($"Failed to delete ticket with Id: {request.Id}");
            }
                
            return result;
        }
    }
}
