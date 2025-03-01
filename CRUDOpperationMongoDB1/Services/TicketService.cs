using MongoDB.Driver;
using TicketAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CRUDOpperationMongoDB1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

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
        public async Task<List<Ticket>> GetAllsync()
        {
            List<Ticket>? tickets = await _tickets.Find(t => true).ToListAsync();
            Console.WriteLine($"📌 [INFO] Số lượng vé lấy được: {tickets?.Count ?? 0}");
            return tickets;
        }

        // Lấy một ticket theo Id
        public async Task<Ticket> GetByIdAsync(string id) =>
               await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();

        // Tạo một ticket mới
        public async Task CreateAsync(Ticket ticket) =>
            await _tickets.InsertOneAsync(ticket);


        public async Task UpdateAsync(string id, Ticket ticket)
        {
            var existingTicket = await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (existingTicket == null)
            {
                throw new Exception("Ticket not found");
            }

            // Cập nhật thông tin vé
            await _tickets.ReplaceOneAsync(t => t.Id == id, ticket);
        }
        // Cập nhật trạng thái của ticket theo Id
        public async Task UpdateStatusAsync(string id, string status)
        {
            var update = Builders<Ticket>.Update.Set(t => t.Status, status); // Tạo update cho trường Status
            await _tickets.UpdateOneAsync(t => t.Id == id, update); // Thực hiện update trong database
        }
        public async Task<List<Ticket>> Find(Expression<Func<Ticket, bool>> filter)
        {
            return await _tickets.Find(filter).ToListAsync();
        }
        //
        public async Task InsertManyAsync(List<Ticket> tickets)
        {
            if (tickets == null || tickets.Count == 0)
                return;

            await _tickets.InsertManyAsync(tickets);
        }
        // xóa theo id
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _tickets.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }


    }
}





