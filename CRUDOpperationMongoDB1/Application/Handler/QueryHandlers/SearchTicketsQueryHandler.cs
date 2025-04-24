using MediatR;
using CRUDOpperationMongoDB1.Application.Mapper;
using CRUDOpperationMongoDB1.Application.Queries;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Domain.Enums;
using MongoDB.Driver;
namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    public class SearchTicketsQueryHandler : IRequestHandler<SearchTicketsQuery, List<TicketDto>>
    {
        private readonly ITicketRepository _TicketRepository;

        public SearchTicketsQueryHandler(ITicketRepository ticketRepository)
        {
            _TicketRepository = ticketRepository;
        }

        public async Task<List<TicketDto>> Handle(SearchTicketsQuery request, CancellationToken cancellationToken)
        {
            // Khởi tạo các bộ lọc
            var builder = Builders<Ticket>.Filter;
            var filter = builder.Gte(t => t.FromDate, request.FromDate ?? DateTime.UtcNow.AddDays(-7));
            if (!string.IsNullOrEmpty(request.FromAddress))
            {
                filter &= builder.Eq(t => t.FromAddress, request.FromAddress);
            }

            if (!string.IsNullOrEmpty(request.ToAddress))
            {
                filter &= builder.Eq(t => t.ToAddress, request.ToAddress);
            }

            if (request.TicketType == TicketType.KhuHoi || request.TicketType == TicketType.MotChieu)
            {
                filter &= builder.Eq(t => t.TicketType,request.TicketType);
            }

            if (request.Status == TicketStatus.Active || request.Status == TicketStatus.Inactive || request.Status == TicketStatus.Deleted)
            {
                filter &= builder.Eq(t => t.Status, request.Status);
            }
            if (request.ToDate.HasValue)
            {
                filter &= builder.Lte(t => t.ToDate, request.ToDate.Value);
            }

            var tickets = await _TicketRepository.FindTicketsAsync(filter);

            return tickets.Select(TicketMapper.ToDto).ToList();
        }
    }
}
