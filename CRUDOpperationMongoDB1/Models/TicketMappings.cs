using System.Net.Sockets;
using TicketAPI.DTOs;
using TicketAPI.Models;

namespace TicketAPI.Mappings
{
    public static class TicketMapper
    {
        public static Ticket ToEntity(CreateTicketDTO dto)
        {
            return new Ticket
            {
                TicketType = (TicketType)Enum.Parse(typeof(TicketType), dto.TicketType), // Chuyển enum thành string
                FromAddress = dto.FromAddress,
                ToAddress = dto.ToAddress,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                Quantity = dto.Quantity,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                Status = (TicketStatus)Enum.Parse(typeof(TicketStatus), dto.Status)
            };
        }

        public static CreateTicketDTO ToDTO(Ticket ticket)
        {
            return new CreateTicketDTO
            {
                TicketType = ticket.TicketType.ToString(), 
                FromAddress = ticket.FromAddress,
                ToAddress = ticket.ToAddress,
                FromDate = ticket.FromDate,
                ToDate = ticket.ToDate,
                Quantity = ticket.Quantity,
                CustomerName = ticket.CustomerName,
                CustomerPhone = ticket.CustomerPhone,
                Status = ticket.Status.ToString()
            };
        }
    }
}
