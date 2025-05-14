using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Domain.Enums;

namespace CRUDOpperationMongoDB1.Application.Mapper
{
    public static class TicketMapper
    {
        public static TicketDto ToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                TicketType = Enum.GetName(typeof(TicketType), ticket.TicketType),
                FromAddress = ticket.FromAddress,
                ToAddress = ticket.ToAddress,
                FromDate = ticket.FromDate,
                ToDate = ticket.ToDate,
                Quantity = ticket.Quantity,
                CustomerName = ticket.CustomerName,
                CustomerPhone = ticket.CustomerPhone,
                Status = Enum.GetName(typeof(TicketStatus), ticket.Status)
            };

        }
        public static Ticket ToEntity(CreateTicketCommand command, Customer customer)
        {
            return new Ticket
            {

                TicketType = (TicketType)Enum.Parse(typeof(TicketType), command.TicketType),
                FromAddress = command.FromAddress,
                ToAddress = command.ToAddress,
                FromDate = command.FromDate,
                ToDate = command.ToDate,
                Quantity = command.Quantity,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                Status = (TicketStatus)Enum.Parse(typeof(TicketStatus), command.Status)
            };
        }


    }
}
