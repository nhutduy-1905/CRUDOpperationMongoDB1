using CRUDOpperationMongoDB1.Application.Command.Tickets;
using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Data;
using CRUDOpperationMongoDB1.Domain.Enums;

namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{

    public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, Ticket>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UpdateTicketCommandHandler> _logger; // them logger de theo doi
        public UpdateTicketCommandHandler(IApplicationDbContext context, ILogger<UpdateTicketCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Ticket> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        { 
            // kiem tra ticket co ton tai trong co so du lieu khong
            var ticket = await _context.Tickets.Find(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (ticket == null)
            {
                _logger.LogWarning($"Ticket with If{request.Id} not found");
                return null;

            }
            // cap nhat thong tin tu yeu cau 
            ticket.TicketType = Enum.Parse<TicketType>(request.TicketType);
            ticket.FromAddress = request.FromAddress;
            ticket.ToAddress = request.ToAddress;
            ticket.FromDate = request.FromDate;
            ticket.ToDate = request.ToDate;
            ticket.Quantity = request.Quantity;
            ticket.CustomerName = request.CustomerName;
            ticket.CustomerPhone = request.CustomerPhone;
            ticket.Status = Enum.Parse<TicketStatus>(request.Status);

            // thuc hien cap nhat ticket vao mongo
            var result = await _context.Tickets.ReplaceOneAsync(
            x => x.Id == request.Id,
            ticket,
            cancellationToken: cancellationToken);

            // kiem tra xem ban ghi co duoc thay dou khong va log ket qua 
            if (result.ModifiedCount > 0)
            {
                _logger.LogInformation($"Ticket with Id {request.Id} successfully updated.");
                return ticket; // ticket da duoc cap nhat
            }
            _logger.LogWarning($"No changes made ti ticket with Id{request.Id}. It might be up to date.");
            return ticket; // khong thay doi tra ve false
        }
    }
}

