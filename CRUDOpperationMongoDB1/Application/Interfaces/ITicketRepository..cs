using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CRUDOpperationMongoDB1.Application.Interfaces
{
    public interface ITicketRepository
    {

        Task<Ticket> GetTicketByIdAsync(string id); // lay ticket theo id
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();  // lay danh sach tat ca cac ticket

        // Lay danh sach ve khi phan trang
        Task<PagedResult<Ticket>> GetPageAsync(int page, int pageSize);
        // Tim ve theo dieu kien cu the
        Task<List<Ticket>> FindTicketsAsync(FilterDefinition<Ticket> filter);

        //new
        Task<List<Ticket>> FindTickets(Expression<Func<Ticket, bool>> filter);  // tim kiem linq
        Task<List<Ticket>>FindTickets(FilterDefinition<Ticket> filter); // tim kiem  mongo
        Task AddTicketAsync(Ticket ticket); // them ticket


        Task<Ticket> UpdateTicketAsync(Ticket ticket); // cap nhat ticket
        Task<bool> DeleteTicketAsync(string id); // xoa ticket theo id
      
     
    }
}