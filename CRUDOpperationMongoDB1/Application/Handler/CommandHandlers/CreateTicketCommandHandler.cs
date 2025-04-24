using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Domain.Entities;
using MediatR;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Mapper;
using CRUDOpperationMongoDB1.Application.Interfaces;
namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger<CreateTicketCommandHandler> _logger;
        public CreateTicketCommandHandler(ITicketRepository ticketRepository, ILogger<CreateTicketCommandHandler> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }
        public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {

            try
            {

                _logger.LogInformation($"Recieved request to create ticket with FromAddress: {request.FromAddress}, ToAddress: {request.ToAddress}");
                // Chuyen tu command sang entity
                var ticket = TicketMapper.ToEntity(request);
                // kiem tra du lieu neu khong hop le
                if (ticket == null)
                {
                    _logger.LogWarning("Ticket entity could not be created from command.");
                    throw new InvalidOperationException("Invalid data, ticket entity could not be created.");
                }
                // luu vao database
                await _ticketRepository.AddTicketAsync(ticket);
                _logger.LogInformation($"Ticket wit ID: {ticket.Id} created successfully.");

                // chuyen tu entity sang dto tra ve client
                var ticketDto = TicketMapper.ToDto(ticket);
                return ticketDto;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while creating the ticket: {ex.Message}");
                // co the xu ly loi theo nhu cau cua ban
                throw;
            }
        }
       
    }
}
