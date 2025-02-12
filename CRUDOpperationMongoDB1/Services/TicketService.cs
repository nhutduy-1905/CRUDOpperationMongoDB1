namespace CRUDOpperationMongoDB1.Services
{
    using CRUDOpperationMongoDB1.Models;
    using System;
    using System.Collections.Generic;

    public class TicketService
    {
        private List<Ticket> tickets = new List<Ticket>(); // Danh sách lưu vé

        class Program
        {
            static void Main()
            {
                TicketService ticketService = new TicketService();



                TicketDTO newTicket = new TicketDTO
                {
                    TicketType = "Khứ hồi",
                    FromAddress = "HCM",
                    ToAddress = "HN",
                    FromDate = "21/01/2025",
                    ToDate = "10/02/2025",
                    Quantity = 1,
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "0988989890"
                };

                Ticket createdTicket = ticketService.CreateTicket(newTicket);

                var tickets = ticketService.GetAllTickets();
                foreach (var t in tickets)
                {
                    Console.WriteLine($"📝 Vé: {t.CustomerName} | {t.FromAddress} -> {t.ToAddress} | {t.FromDate} - {t.ToDate}");
                }
            }
        }

    }

}
