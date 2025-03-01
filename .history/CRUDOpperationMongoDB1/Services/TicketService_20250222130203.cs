using Microsoft.AspNetCore.Mvc;
using TicketAPI.Models;
using TicketAPI.Services;
using TicketAPI.DTOs;
using TicketAPI.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TicketAPI.Controllers
{
    [Route("api/tickets")] // Định nghĩa route chung cho controller
    [ApiController] // Xác định đây là API Controller
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        // Constructor: Inject TicketService để sử dụng trong controller
        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Tạo mới một ticket
        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDTO ticketDto)
        {
            // Kiểm tra dữ liệu đầu vào có hợp lệ không
            if (ticketDto == null || string.IsNullOrEmpty(ticketDto.CustomerPhone))
            {
                return BadRequest(new { error = "Invalid ticket data" });
            }

            var ticket = TicketMapper.ToEntity(ticketDto); // Chuyển DTO thành Entity
            await _ticketService.CreateAsync(ticket); // Gọi service để lưu vào DB

            // Trả về kết quả với thông tin của ticket vừa tạo
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, TicketMapper.ToDTO(ticket));
        }

        // Lấy thông tin một ticket theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(string id)
        {
            var ticket = await _ticketService.GetByIdAsync(id); // Gọi service để tìm ticket theo ID
            if (ticket == null) return NotFound(new { error = "Ticket not found" }); // Nếu không tìm thấy, trả về 404

            return Ok(TicketMapper.ToDTO(ticket)); // Trả về thông tin của ticket dưới dạng DTO
        }

        // Lấy danh sách tất cả ticket (có thể bỏ qua vé đã huỷ)
        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery] bool includeDeleted = false)
        {
            try
            {
                Console.WriteLine("📌 [INFO] Bắt đầu lấy danh sách vé...");

                var tickets = await _ticketService.GetAsync(); // Gọi service lấy danh sách vé
                Console.WriteLine($"📌 [INFO] Số lượng vé lấy được: {tickets?.Count ?? 0}");

                if (tickets == null || tickets.Count == 0)
                {
                    Console.WriteLine("❌ [ERROR] Không tìm thấy vé nào.");
                    return NotFound("Không tìm thấy vé nào.");
                }

                if (!includeDeleted)
                {
                    tickets = tickets.Where(t => t.Status != "Huỷ").ToList(); // Lọc bỏ vé huỷ
                    Console.WriteLine($"📌 [INFO] Vé sau khi lọc: {tickets.Count}");
                }

                var ticketDTOs = tickets.ConvertAll(ticket => TicketMapper.ToDTO(ticket));
                return Ok(ticketDTOs);
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết lỗi
                Console.WriteLine($"❌ [ERROR] Lỗi khi lấy danh sách vé: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách vé.", error = ex.Message });
            }
        }


        // Cập nhật thông tin vé
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateTicket(string id, [FromBody] CreateTicketDTO toDTO)
        {
            // 1. Tìm vé có ID tương ứng
            var existingTicket = await _ticketService.GetByIdAsync(id);
            if (existingTicket == null)
                return NotFound("Không tìm thấy vé!"); // Trả về lỗi nếu vé không tồn tại

            // 2. Kiểm tra ID có khớp không
            if (existingTicket.Id != id)
                return BadRequest("ID không khớp!");

            // 3. Cập nhật thông tin vé    
            existingTicket.TicketType = toDTO.TicketType;
            existingTicket.FromAddress = toDTO.FromAddress;
            existingTicket.ToAddress = toDTO.ToAddress;
            existingTicket.FromDate = toDTO.FromDate;
            existingTicket.ToDate = toDTO.ToDate;
            existingTicket.Quantity = toDTO.Quantity;
            existingTicket.CustomerName = toDTO.CustomerName;
            existingTicket.CustomerPhone = toDTO.CustomerPhone;

            // 4. Lưu thay đổi vào DB
            await _ticketService.UpdateAsync(id, existingTicket);
            return Ok(TicketMapper.ToDTO(existingTicket)); // Trả về thông tin vé sau khi cập nhật
        }

        // Huỷ vé (Cập nhật trạng thái thành "Huỷ")
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> CanCelTicket(string id)
        {
            var ticket = await _ticketService.GetByIdAsync(id); // Lấy vé theo ID
            if (ticket == null) return NotFound("Vé không tồn tại!"); // Nếu không tìm thấy, trả về lỗi 404

            // Cập nhật trạng thái sang "Huỷ"
            await _ticketService.UpdateStatusAsync(id, "Huỷ");
            return Ok(new { message = "Vé đã được huỷ thành công!", ticketId = id }); // Trả về thông báo thành công
        }
    }
}




