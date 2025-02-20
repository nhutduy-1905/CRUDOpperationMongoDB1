using MongoDB.Driver;
using TicketAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CRUDOpperationMongoDB1.Models;

namespace TicketAPI.Services
{
    public class TicketService
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _tickets = database.GetCollection<Ticket>("Tickets");
        }

        public async Task<List<Ticket>> GetAsync() => await _tickets.Find(t => true).ToListAsync();

       public async Task<Ticket> GetByIdAsync(string id) =>
              await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Ticket ticket) =>
            await _tickets.InsertOneAsync(ticket);
    }
}





