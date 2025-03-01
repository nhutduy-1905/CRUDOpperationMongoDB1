using MongoDB.Driver;
using TicketAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CRUDOpperationMongoDB1.Models;
using Microsoft.AspNetCore.Mvc;

namespace TicketAPI.Services
{
    public class TicketService
    {
        private readonly IMongoCollection<Ticket> _tickets;

        // Constructor: Khởi tạo kết nối đến MongoDB
        public TicketService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString); // Kết nối đến MongoDB
            var database = client.GetDatabase(settings.Value.DatabaseName); // Lấy database
            _tickets = database.GetCollection<Ticket>("Tickets"); // Lấy collection "Tickets"
        }

        // Lấy tất cả các ticket từ database
        public async Task<List<Ticket>> GetAsync() => await _tickets.Find(t => true).ToListAsync();

        // Lấy một ticket theo Id
        public async Task<Ticket> GetByIdAsync(string id) =>
               await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();

        // Tạo một ticket mới
        public async Task CreateAsync(Ticket ticket) =>
            await _tickets.InsertOneAsync(ticket);


        // Cập nhật trạng thái của ticket theo Id
        public async Task UpdateStatusAsync(string id, string status)
        {
            var update = Builders<Ticket>.Update.Set(t => t.Status, status); // Tạo update cho trường Status
            await _tickets.UpdateOneAsync(t => t.Id == id, update); // Thực hiện update trong database
        }
    }
}





