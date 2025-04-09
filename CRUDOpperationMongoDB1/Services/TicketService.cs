using System.Linq.Expressions;
using CRUDOpperationMongoDB1.Mappings;
using CRUDOpperationMongoDB1.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TicketAPI.DTOs;
using TicketAPI.Models;

namespace TicketAPI.Services
{
    // Complete
    public class TicketService : ITicketService
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _tickets = database.GetCollection<Ticket>("Tickets");
        }

        // ✅ Lấy tất cả ticket
        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            var tickets = await _tickets.Find(ticket => true).ToListAsync();
            foreach (var ticket in tickets)
            {
                ticket.Status = (TicketStatus)ticket.Status;
            }

            return tickets;
        }

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            List<Ticket>? tickets = await _tickets.Find(t => true).ToListAsync();
            Console.WriteLine($"📌 [INFO] Số lượng vé lấy được: {tickets?.Count ?? 0}");
            return tickets;
        }
        // ✅ Lấy ticket theo ID
        public async Task<Ticket> GetTicketByIdAsync(string id) =>
               await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();
        
        // ✅ Lấy tất cả vé (Hỗ trợ phân trang)
        public async Task<List<Ticket>> GetAllTicketsAsync(int page = 1, int pageSize = 10)
        {
            return await _tickets.Find(t => true)
                                 .Skip((page - 1) * pageSize)
                                 .Limit(pageSize)
                                 .ToListAsync();
        }
        // ✅ Lấy danh sách vé theo ID khách hàng (phân trang)
        public async Task<List<Ticket>> GetTicketsByCustomerIdAsync(string customerId, int page, int pageSize)
        {
            return await _tickets.Find(t => t.CustomerId == customerId)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }
        // ✅ Lọc vé hỗ trợ tìm kiếm
        public async Task<List<Ticket>> FindTickets(FilterDefinition<Ticket> filter)
        {
            return await _tickets.Find(filter).ToListAsync();
        }

        // ✅ Tìm vé theo bộ lọc
        public async Task<List<Ticket>> FindTickets(Expression<Func<Ticket, bool>> filter)
        {
            return await _tickets.Find(filter).ToListAsync();
        }
        public async Task<long> CountByTicketStatus(TicketStatus status)
        {
            var countByStatus = await _tickets.CountAsync(f => f.Status == status);
            return countByStatus;
        }

        // ✅ Tạo ticket mới
        public async Task<Ticket> InsertOneTicketAsync(Ticket ticket)
        {
            try
            {
                await _tickets.InsertOneAsync(ticket);
                return ticket;
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                Console.WriteLine($"Lỗi khi gọi CreateAsync: {ex.GetType().Name} - {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw; // Ném lại ngoại lệ để CreateTicket xử lý
            }
        }
        // ✅ Thêm nhiều vé
        public async Task InsertManyTicketsAsync(List<Ticket> tickets)
        {
            if (tickets == null || tickets.Count == 0)
                return;

            await _tickets.InsertManyAsync(tickets);
        }

        // ✅ Cập nhật ticket
        public async Task UpdateTicketAsync(string id, UpdateTicketDTO dto)
        {
            var existingTicket = await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (existingTicket == null)
            {
                throw new Exception("Ticket không tồn tại!");
            }
            var ticket = TicketMapper.UpdateTicketDTOToEntity(dto);
            await _tickets.ReplaceOneAsync(t => t.Id == id, ticket);
        }
        // ✅ Cập nhật trạng thái ticket
        public async Task UpdateTicketStatusAsync(string id, string status)
        {
            var update = Builders<Ticket>.Update.Set(t => t.Status, (TicketStatus)Enum.Parse(typeof(TicketStatus), status));
            await _tickets.UpdateOneAsync(t => t.Id == id, update);
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
                //CustomerName = t.CustomerName,
                //CustomerPhone = t.CustomerPhone,
                Status = ((TicketStatus)(int)t.Status).ToString() // ✅ Chuyển đổi ENUM thành chuỗi
            }).ToList();
        }
        // ✅ Xóa vé theo ID
        public async Task<bool> DeleteTicketAsync(string id)
        {
            var result = await _tickets.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }       
    }
}
