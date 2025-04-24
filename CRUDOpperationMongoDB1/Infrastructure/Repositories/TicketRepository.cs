using CRUDOpperationMongoDB1.Data;
using MongoDB.Driver;
using CRUDOpperationMongoDB1.Application.Interfaces;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CRUDOpperationMongoDB1.Models;
using MongoDB.Driver.Linq;
namespace CRUDOpperationMongoDB1.Domain.Entities;
public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Ticket> _tickets;

    public TicketRepository(IApplicationDbContext context)
    {
        _tickets = context.Tickets;
    }
    public async Task<Ticket> GetTicketByIdAsync(string id)
    {
        return await _tickets.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _tickets.Find(_ => true).ToListAsync();
    }
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
    public async Task<List<Ticket>> FindTicketsAsync(FilterDefinition<Ticket> filter)
    {
        return await _tickets.Find(filter).ToListAsync();
    }
    // new
    public async Task<List<Ticket>> FindTickets(Expression<Func<Ticket, bool>> filter)
    {
        return await _tickets.AsQueryable().Where(filter).ToListAsync();
    }
    public async Task<List<Ticket>> FindTickets(FilterDefinition<Ticket> filter)
    {
        return await _tickets.Find(filter).ToListAsync();
    }
    public async Task AddTicketAsync(Ticket ticket)
    {
        await _tickets.InsertOneAsync(ticket);
    }
    public async Task<Ticket> UpdateTicketAsync(Ticket ticket)
    {
        var result = await _tickets.ReplaceOneAsync(x => x.Id == ticket.Id, ticket);
        if (result.ModifiedCount > 0)
        {
            return ticket;
        }
        return null;
    }
    public async Task<bool> DeleteTicketAsync(string id)
    {
        var result = await _tickets.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
