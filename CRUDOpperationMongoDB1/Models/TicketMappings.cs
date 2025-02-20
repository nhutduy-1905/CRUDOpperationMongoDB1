using TicketAPI.Models;
using TicketAPI.DTOs;

namespace TicketAPI.Mappings
{
    public static class TicketMapper
    {
        public static Ticket ToEntity(CreateTicketDTO dto)
        {
            return new Ticket
            {
                TicketType = dto.TicketType,
                FromAddress = dto.FromAddress,
                ToAddress = dto.ToAddress,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                Quantity = dto.Quantity,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone
            };
        }

        public static CreateTicketDTO ToDTO(Ticket entity)
        {
            return new CreateTicketDTO
            {
                TicketType = entity.TicketType,
                FromAddress = entity.FromAddress,
                ToAddress = entity.ToAddress,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                Quantity = entity.Quantity,
                CustomerName = entity.CustomerName,
                CustomerPhone = entity.CustomerPhone
            };
        }
    }
}
