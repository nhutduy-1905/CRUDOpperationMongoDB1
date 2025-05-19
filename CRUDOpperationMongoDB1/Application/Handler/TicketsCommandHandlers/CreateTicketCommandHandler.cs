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
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CreateTicketCommandHandler> _logger;
        public CreateTicketCommandHandler(ITicketRepository ticketRepository, ICustomerRepository customerRepository, ILogger<CreateTicketCommandHandler> logger)
        {
            _ticketRepository = ticketRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }
        public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation($"Received request to create ticket for CustomerId: {request.CustomerId}");

                // Tìm khách hàng từ repository
                var customerResult = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);

                // Kiểm tra nếu không tìm thấy khách hàng
                if (customerResult.IsSuccess || customerResult.Data == null)
                {
                    _logger.LogWarning("Customer not found.");
                    throw new InvalidOperationException("Customer not found.");
                }

                // 3. Unwrap ra Customer thật
                var customer = customerResult.Data;
                // Tạo vé từ command và customer
                var ticket = TicketMapper.ToEntity(request, customer);

                // Lưu vé vào repository
                await _ticketRepository.AddTicketAsync(ticket);
                _logger.LogInformation($"Ticket with ID: {ticket.Id} created successfully.");

                // Trả về DTO của vé
                return TicketMapper.ToDto(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating the ticket: {ex.Message}");
                throw;
            }
        }
    }
}
        
