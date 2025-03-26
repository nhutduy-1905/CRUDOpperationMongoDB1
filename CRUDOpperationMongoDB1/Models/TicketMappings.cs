using System.Net.Sockets;
using TicketAPI.DTOs;
using TicketAPI.Models;

namespace TicketAPI.Mappings
{
    public static class TicketMapper
    {
        public static Ticket ToEntity(CreateTicketDTO dto)
        {
            if(dto == null) 
                throw new ArgumentNullException(nameof(dto));

                return new Ticket
                {
                    TicketType = Enum.TryParse<TicketType>(dto.TicketType, true, out var ticketType)
                    ?ticketType
                    :throw new ArgumentException("Ivalid TicketTupe Value"),// Chuyển enum thành string
                    FromAddress = dto.FromAddress,
                    ToAddress = dto.ToAddress,
                    FromDate = dto.FromDate,
                    ToDate = dto.ToDate,
                    Quantity = dto.Quantity,
                    CustomerId = dto.CustomerId,
                    //CustomerName = dto.CustomerName,
                    //CustomerPhone = dto.CustomerPhone,
                    Status = (TicketStatus)Enum.Parse(typeof(TicketStatus), dto.Status)
            }; 
        }

        public static CreateTicketDTO ToDTO(Ticket ticket)
        {
            if(ticket == null) throw new ArgumentNullException(nameof (ticket));
            return new CreateTicketDTO
            { 
                TicketType = ticket.TicketType.ToString(), 
                FromAddress = ticket.FromAddress,
                ToAddress = ticket.ToAddress,
                FromDate = ticket.FromDate,
                ToDate = ticket.ToDate,
                Quantity = ticket.Quantity,
                CustomerId = ticket.CustomerId,
                //CustomerName = ticket.CustomerName,
                //CustomerPhone = ticket.CustomerPhone,
                Status = ticket.Status.ToString()
            };
        }
    }
}

