using Microsoft.AspNetCore.Mvc;
using TicketAPI.Models;
using TicketAPI.Services;
using TicketAPI.DTOs;
using TicketAPI.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.ComponentModel;
using OfficeOpenXml;
using MongoDB.Bson;

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

        // Lấy tất cả vé (Fix lỗi status 500)
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var tickets = await _ticketService.GetAsync();
                if (tickets == null || tickets.Count == 0)
                    return NotFound(new { message = "Không có vé nào!" });

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 [ERROR] {ex.Message}");
                Console.WriteLine($"🔥 StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔥 InnerException: {ex.InnerException.Message}");
                }

                return StatusCode(500, new
                {
                    error = "Lỗi khi lấy dữ liệu từ MongoDB",
                    details = ex.Message,
                    stackTrace = ex.StackTrace
                });
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

        [HttpGet("export")]
        public async Task<IActionResult> ExportTicketsToExcel()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var tickets = await _ticketService.Find(_ => true); // Lấy toàn bộ ticket từ MongoDB

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Tickets");

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Loại Vé";
                worksheet.Cells[1, 3].Value = "Điểm Đi";
                worksheet.Cells[1, 4].Value = "Điểm Đến";
                worksheet.Cells[1, 5].Value = "Ngày Đi";
                worksheet.Cells[1, 6].Value = "Ngày Đến";
                worksheet.Cells[1, 7].Value = "Số Lượng";
                worksheet.Cells[1, 8].Value = "Tên Khách Hàng";
                worksheet.Cells[1, 9].Value = "SĐT Khách Hàng";
                worksheet.Cells[1, 10].Value = "Trạng Thái";

                // Ghi dữ liệu từ MongoDB vào file Excel
                int row = 2;
                foreach (var ticket in tickets)
                {
                    worksheet.Cells[row, 1].Value = ticket.Id;
                    worksheet.Cells[row, 2].Value = ticket.TicketType;
                    worksheet.Cells[row, 3].Value = ticket.FromAddress;
                    worksheet.Cells[row, 4].Value = ticket.ToAddress;
                    worksheet.Cells[row, 5].Value = ticket.FromDate.ToString("yyyy-MM-dd HH:mm");
                    worksheet.Cells[row, 6].Value = ticket.ToDate.ToString("yyyy-MM-dd HH:mm");
                    worksheet.Cells[row, 7].Value = ticket.Quantity;
                    worksheet.Cells[row, 8].Value = ticket.CustomerName;
                    worksheet.Cells[row, 9].Value = ticket.CustomerPhone;
                    worksheet.Cells[row, 10].Value = ticket.Status;
                    row++;
                }

                // Lưu file Excel vào MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tickets.xlsx");
            }
        }
        [HttpPost("import")]
        public async Task<IActionResult> ImportTicketsFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Vui lòng chọn file Excel.");
            }
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var tickets = new List<Ticket>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        return BadRequest("File Excel không có dữ liệu hoặc sai định dạng!");
                    }
                    int rowCount = worksheet.Dimension.Rows; // Đếm số dòng

                    for (int row = 2; row <= rowCount; row++) // Bỏ qua dòng tiêu đề
                    {
                        var ticket = new Ticket
                        {
                            Id = ObjectId.GenerateNewId().ToString(), // Tạo ID mới nếu cần
                            TicketType = worksheet.Cells[row, 2].Text,
                            FromAddress = worksheet.Cells[row, 3].Text,
                            ToAddress = worksheet.Cells[row, 4].Text,
                            FromDate = DateTime.Parse(worksheet.Cells[row, 5].Text),
                            ToDate = DateTime.Parse(worksheet.Cells[row, 6].Text),
                            Quantity = int.Parse(worksheet.Cells[row, 7].Text),
                            CustomerName = worksheet.Cells[row, 8].Text,
                            CustomerPhone = worksheet.Cells[row, 9].Text,
                            Status = worksheet.Cells[row, 10].Text
                        };

                        tickets.Add(ticket);
                    }
                }
            }

            // Lưu vào MongoDB
            await _ticketService.InsertManyAsync(tickets);

            return Ok(new { Message = "Import thành công!", Total = tickets.Count });
        }
        [HttpDelete("deleted/{id}")]
        public async Task<IActionResult> DeleteTicket(string id)
        {
            bool isDeleted = await _ticketService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound(new { message = $"Không tìm thấy vé với ID: {id}" });
            }

            return Ok(new { message = $"Đã xóa vé với ID: {id}" });
        }

    }
}


