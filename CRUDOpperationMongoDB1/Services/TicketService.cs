using MongoDB.Driver;
using TicketAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CRUDOpperationMongoDB1.Models;
using System.Linq.Expressions;
using TicketAPI.DTOs;

using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

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

        // ✅ Lấy tất cả ticket
        public async Task<List<Ticket>> GetAsync()
        {
            var tickets = await _tickets.Find(ticket => true).ToListAsync();
            foreach (var ticket in tickets)
            {
                ticket.Status = (TicketStatus)ticket.Status;
            }

            return tickets;
        }

        public async Task<List<Ticket>> GetAllsync()
        {
            List<Ticket>? tickets = await _tickets.Find(t => true).ToListAsync();
            Console.WriteLine($"📌 [INFO] Số lượng vé lấy được: {tickets?.Count ?? 0}");
            return tickets;
        }

        // ✅ Lấy ticket theo ID
        public async Task<Ticket> GetByIdAsync(string id) =>
               await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();

        // ✅ Tạo ticket mới
        public async Task CreateAsync(Ticket ticket) =>
            await _tickets.InsertOneAsync(ticket);

        // ✅ Cập nhật ticket
        public async Task UpdateAsync(string id, Ticket ticket)
        {
            var existingTicket = await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (existingTicket == null)
            {
                throw new Exception("Ticket không tồn tại!");
            }

            await _tickets.ReplaceOneAsync(t => t.Id == id, ticket);
        }

        // ✅ Cập nhật trạng thái ticket
        public async Task UpdateStatusAsync(string id, string status)
        {
            var update = Builders<Ticket>.Update.Set(t => t.Status, (TicketStatus)Enum.Parse(typeof(TicketStatus), status));
            await _tickets.UpdateOneAsync(t => t.Id == id, update);
        }

        // ✅ Tìm vé theo bộ lọc
        public async Task<List<Ticket>> Find(Expression<Func<Ticket, bool>> filter)
        {
            return await _tickets.Find(filter).ToListAsync();
        }

        // ✅ Thêm nhiều vé
        public async Task InsertManyAsync(List<Ticket> tickets)
        {
            if (tickets == null || tickets.Count == 0)
                return;

            await _tickets.InsertManyAsync(tickets);
        }

        // ✅ Xóa vé theo ID
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _tickets.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }

        // ✅ Lọc vé hỗ trợ tìm kiếm
        public async Task<List<Ticket>> Find(FilterDefinition<Ticket> filter)
        {
            return await _tickets.Find(filter).ToListAsync();
        }

        // ✅ Lấy tất cả vé (Hỗ trợ phân trang)
        public async Task<List<Ticket>> GetAllTicketsAsync(int page = 1, int pageSize = 10)
        {
            return await _tickets.Find(t => true)
                                 .Skip((page - 1) * pageSize)
                                 .Limit(pageSize)
                                 .ToListAsync();
        }

        public async Task<List<CreateTicketDTO>> UpdateTicketStatusAsync(List<UpdateTicketStatusDTO> updates)
        {
            if (updates == null || !updates.Any())
                throw new ArgumentException("🚨 Danh sách cập nhật trống!");

            var bulkOperations = new List<WriteModel<Ticket>>();

            foreach (var update in updates)
            {
                if (Enum.TryParse<TicketStatus>(update.Status.ToString(), out var ticketStatusEnum))
                {
                    var filter = Builders<Ticket>.Filter.Eq(t => t.Id, update.Id);
                    var updateDef = Builders<Ticket>.Update.Set(t => t.Status, ticketStatusEnum);
                    bulkOperations.Add(new UpdateOneModel<Ticket>(filter, updateDef));
                }

            }

            if (bulkOperations.Count > 0)
                await _tickets.BulkWriteAsync(bulkOperations);

            // Lấy lại danh sách vé sau khi cập nhật
            var updatedTickets = await _tickets.Find(t => updates.Select(u => u.Id).Contains(t.Id)).ToListAsync();

            // Trả về danh sách với Status hiển thị dạng chuỗi
            return updatedTickets.Select(t => new CreateTicketDTO
            {
                TicketType = t.TicketType.ToString(),
                FromAddress = t.FromAddress,
                ToAddress = t.ToAddress,
                FromDate = t.FromDate,
                ToDate = t.ToDate,
                Quantity = t.Quantity,
                CustomerName = t.CustomerName,
                CustomerPhone = t.CustomerPhone,
                Status = ((TicketStatus)(int)t.Status).ToString() // ✅ Chuyển đổi ENUM thành chuỗi
            }).ToList();
        }
    }
}
