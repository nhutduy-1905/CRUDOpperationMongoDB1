using CRUDOpperationMongoDB1.Data;
using MongoDB.Driver;
using CRUDOpperationMongoDB1.Application.Interfaces;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CRUDOpperationMongoDB1.Models;
using MongoDB.Driver.Linq;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Domain.Enums;
namespace CRUDOpperationMongoDB1.Domain.Entities;
public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Ticket> _tickets;

    public TicketRepository(IApplicationDbContext context)
    {
        _tickets = context.Tickets;
     
    }
    // lay ve theo id
    public async Task<Ticket> GetTicketByIdAsync(string id)
    {
        return await _tickets.Find(ticket => ticket.Id == id).FirstOrDefaultAsync();
    }
    // Lay tat ca ve
    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _tickets.Find(_ => true).ToListAsync();
    }
    // lay danh sach ve voi phan trang
    public async Task<PagedResult<Ticket>> GetPageAsync(int page, int pageSize)
    {
        var totalCount = await _tickets.CountDocumentsAsync(_ => true);
        var tickets = await _tickets
            .Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
        return new PagedResult<Ticket>
        {
            Items = tickets,
            TotalCount = (int)totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    // Triển khai phương thức GetTicketsByFilterAsync
    public async Task<List<Ticket>> GetTicketsByFilterAsync(TicketDto filter)
    {
        // Xây dựng bộ lọc
        var builder = Builders<Ticket>.Filter;
        var ticketFilter = builder.Empty; // Mặc định là không có bộ lọc

        // Áp dụng các điều kiện lọc từ filter
        if (!string.IsNullOrEmpty(filter.TicketType))
        {
            ticketFilter &= builder.Eq("TicketType", filter.TicketType); // Dùng tên trường thay vì biểu thức lambda
        }
        if (!string.IsNullOrEmpty(filter.FromAddress))
        {
            ticketFilter &= builder.Eq(t => t.FromAddress, filter.FromAddress);
        }
        if (!string.IsNullOrEmpty(filter.ToAddress))
        {
            ticketFilter &= builder.Eq(t => t.ToAddress, filter.ToAddress);
        }
        if (filter.FromDate != default(DateTime))
        {
            ticketFilter &= builder.Gte(t => t.FromDate, filter.FromDate);
        }
        if (filter.ToDate != default(DateTime))
        {
            ticketFilter &= builder.Lte(t => t.ToDate, filter.ToDate);
        }
        if (filter.Quantity > 0)
        {
            ticketFilter &= builder.Eq(t => t.Quantity, filter.Quantity);
        }
        if (!string.IsNullOrEmpty(filter.CustomerName))
        {
            ticketFilter &= builder.Eq(t => t.CustomerName, filter.CustomerName);
        }
        if (!string.IsNullOrEmpty(filter.CustomerPhone))
        {
            ticketFilter &= builder.Eq(t => t.CustomerPhone, filter.CustomerPhone);
        }
        if (!string.IsNullOrEmpty(filter.Status))
        {
            ticketFilter &= builder.Eq("Status", filter.Status); // Sử dụng tên trường trực tiếp
        }
        Console.WriteLine(ticketFilter);  // In ra bộ lọc để kiểm tra
        // Thực thi truy vấn và trả về kết quả
        var tickets = await _tickets.Find(ticketFilter).ToListAsync();
        return tickets;
    }
    public async Task<Ticket?> GetByIdAsync(string id)
    {
        return await _tickets.Find(ticket => ticket.Id == id).FirstOrDefaultAsync();
    }
    public async Task<List<Ticket>> UpdateStatusBulkAsync(List<UpdateTicketStatusDTO> updates)
    {
        var ticketIds = updates.Select(u => u.TicketId).ToList();
        // Dam bao await khi goi phuong thuc bat dong bo
        var tickets =  await _tickets.Find(t => ticketIds.Contains(t.Id)).ToListAsync();

        foreach (var ticket in tickets)
        {
            var update = updates.FirstOrDefault(u => u.TicketId == ticket.Id);
            if (update != null)
            {
                ticket.Status = update.Status;
                ticket.TicketType = update.Type;
            }
        }
        foreach (var ticket in tickets)
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t.Id, ticket.Id);
            await _tickets.ReplaceOneAsync(filter, ticket);
        }
        return tickets;
    }
    public async Task UpdateStatusAsync(string id, TicketStatus status)
    {
         var ticket = await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();
        if (ticket != null)
        {
            ticket.Status = status;
            var filter = Builders<Ticket>.Filter.Eq(t => t.Id, ticket.Id);
            await _tickets.ReplaceOneAsync(filter, ticket); // cap nhat ve
        }
    }
    //  them ve moi
    public async Task InsertOneTicketAsync(Ticket ticket)
    {
        await _tickets.InsertOneAsync(ticket);
    }
    // cap nhat ve
    public async Task UpdateTicketAsync(string id, Ticket ticket)
    {
        var filter = Builders<Ticket>.Filter.Eq(t => t.Id, id);
        await _tickets.ReplaceOneAsync(filter, ticket);
    }
    // tim  ve theo filter (MONGODB)
    public async Task<List<Ticket>> FindTicketsAsync(FilterDefinition<Ticket> filter)
    {
        return await _tickets.Find(filter).ToListAsync();
    }
    // tim ve theo filter (LINQ)
    public async Task<List<Ticket>> FindTickets(Expression<Func<Ticket, bool>> filter)
    {
        return await _tickets.AsQueryable().Where(filter).ToListAsync();
    }
    public async Task<List<Ticket>> FindTickets(FilterDefinition<Ticket> filter)
    {
        return await _tickets.Find(filter).ToListAsync();
    }
    // them ve
    public async Task AddTicketAsync(Ticket ticket)
    {
        await _tickets.InsertOneAsync(ticket);
    }
    // cap nhat ve
    public async Task<Ticket> UpdateTicketAsync(Ticket ticket)
    {
        var result = await _tickets.ReplaceOneAsync(x => x.Id == ticket.Id, ticket);
        if (result.ModifiedCount > 0)
        {
            return ticket;
        }
        return null;
    }
    // xoa ve theo ID
    public async Task<bool> DeleteTicketAsync(string id)
    {
        var result = await _tickets.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
