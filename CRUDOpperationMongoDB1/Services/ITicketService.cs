using System.Linq.Expressions;
using MongoDB.Driver;
using TicketAPI.DTOs;
using TicketAPI.Models;

namespace TicketAPI.Services
{
    // Format tên file đúng  ✔️
    // Đặt đúng vị trí chức năng 
    // Đồng nhất cách tổ chức input + output
    public interface ITicketService
    {
       Task<List<Ticket>> GetTicketsAsync();
       Task<List<Ticket>> GetAllTicketsAsync();
       Task<List<Ticket>> GetAllTicketsAsync(int page = 1, int pageSize = 10);
       Task<List<Ticket>> GetTicketsByCustomerIdAsync(string customerId, int page, int pageSize);
       Task<Ticket> GetTicketByIdAsync(string id);
       Task<List<Ticket>> FindTickets(FilterDefinition<Ticket> filter);
       Task<List<Ticket>> FindTickets(Expression<Func<Ticket, bool>> filter);
        Task<long> CountByTicketStatus(TicketStatus status);

       Task<Ticket> InsertOneTicketAsync(Ticket ticket);
       Task InsertManyTicketsAsync(List<Ticket> tickets);

       Task UpdateTicketAsync(string id, UpdateTicketDTO ticket);
       Task UpdateTicketStatusAsync(string id, string status);
       Task<List<CreateTicketDTO>> UpdateTicketStatusAsync(List<UpdateTicketStatusDTO> updates);
       
       Task<bool> DeleteTicketAsync(string id);
    }
}
