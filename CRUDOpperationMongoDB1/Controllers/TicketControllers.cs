using Microsoft.AspNetCore.Mvc;
using TicketAPI.Models;
using TicketAPI.Services;
using TicketAPI.DTOs;
using TicketAPI.Mappings;
using MongoDB.Driver;
using OfficeOpenXml;
using MongoDB.Bson;
using OfficeOpenXml.Style;
using TicketAPI.Service;
using System;

namespace TicketAPI.Controllers
{
    [Route("api/tickets")] // Định nghĩa route chung cho controller
    [ApiController] // Xác định đây là API Controller
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly CustomerService _customerService;
        // Constructor: Inject TicketService để sử dụng trong controller
        public TicketController(TicketService ticketService, CustomerService customerService)
        {
            _ticketService = ticketService;
            _customerService = customerService;
        }

        // Tạo mới một ticket
        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDTO ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ticketDto == null || string.IsNullOrEmpty(ticketDto.CustomerId))
            {
                return BadRequest(new { error = "Invalid ticket data: CustomerId is required." });
            }
            try
            {
               
                // Kiểm tra CustomerId có tồn tại không
                Customer? customer = await _customerService.GetCustomerByIdAsync(ticketDto.CustomerId);
                if (customer == null)
                {
                    return BadRequest(new { error = "CustomerId không tồn tại trong hệ thống." });
                }
                // Tạo entity Ticket từ DTO, bổ sung CustomerName và CustomerPhone từ customer
                var tickets = TicketMapper.ToEntity(ticketDto);
                tickets.CustomerName = customer.CustomerName;
                tickets.CustomerPhone = customer.CustomerPhone;
                // Lưu ticket vào database
                var ticketResult = await _ticketService.CreateAsync(tickets);
                // trả về kq vừa tạo
                return Ok(new { success = true, message = "Tạo vé thành công!", data = ticketResult });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo vé: {ex.GetType().Name} - {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Lỗi hệ thống. Vui lòng thử lại sau!" });
            }
        
            //var ticket = TicketMapper.ToEntity(ticketDto); // Chuyển DTO thành Entity
            //await _ticketService.CreateAsync(ticket); // Gọi service để lưu vào DB

            //// Trả về kết quả với thông tin của ticket vừa tạo
            //return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, TicketMapper.ToDTO(ticket));
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
            existingTicket.TicketType = Enum.Parse<TicketType>(toDTO.TicketType);
            existingTicket.FromAddress = toDTO.FromAddress;
            existingTicket.ToAddress = toDTO.ToAddress; 
            existingTicket.FromDate = toDTO.FromDate;
            existingTicket.ToDate = toDTO.ToDate;
            existingTicket.Quantity = toDTO.Quantity;
            //existingTicket.CustomerName = toDTO.CustomerName;
            //existingTicket.CustomerPhone = toDTO.CustomerPhone;

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
                        var ticketId = worksheet.Cells[row, 1].Text.Trim();
                        var ticketType = worksheet.Cells[row, 2].Text.Trim();
                        var fromAddress = worksheet.Cells[row, 3].Text.Trim();
                        var toAddress = worksheet.Cells[row, 4].Text.Trim();
                        var fromDateText = worksheet.Cells[row, 5].Text.Trim();
                        var toDateText = worksheet.Cells[row, 6].Text.Trim();
                        var quantityText = worksheet.Cells[row, 7].Text.Trim();
                        var customerName = worksheet.Cells[row, 8].Text.Trim();
                        var customerPhone = worksheet.Cells[row, 9].Text.Trim();
                        var status = worksheet.Cells[row, 10].Text.Trim();

                        // 🔹 Chuyển đổi TicketType (string -> Enum)
                        TicketType? ticketTypeEnum = null;
                        if (!string.IsNullOrWhiteSpace(ticketType) &&
                            Enum.TryParse<TicketType>(ticketType, true, out var parsedTicketType))
                        {
                            ticketTypeEnum = parsedTicketType;
                        }
                        else
                        {
                            return BadRequest($"Lỗi tại dòng {row}: TicketType không hợp lệ ({ticketType}).");
                        }



                        TicketStatus? ticketStatus = null;
                        if (!string.IsNullOrWhiteSpace(status) &&
                            Enum.TryParse<TicketStatus>(status , true, out var parsedStatus))
                        {
                            ticketStatus = parsedStatus;
                        }
                        else
                        {
                            return BadRequest($"Lỗi tại dòng {row}: TicketStatus không hợp lệ ({status}).");
                        }

                        // 🔹 Kiểm tra và chuyển đổi các giá trị số
                        if (!DateTime.TryParse(fromDateText, out DateTime fromDate))
                        {
                            return BadRequest($"Lỗi tại dòng {row}: FromDate không hợp lệ ({fromDateText}).");
                        }

                        if (!DateTime.TryParse(toDateText, out DateTime toDate))
                        {
                            return BadRequest($"Lỗi tại dòng {row}: ToDate không hợp lệ ({toDateText}).");
                        }

                        if (!int.TryParse(quantityText, out int quantity))
                        {
                            return BadRequest($"Lỗi tại dòng {row}: Quantity không hợp lệ ({quantityText}).");
                        }

                        // 🔹 Kiểm tra ID có rỗng không
                        Ticket existingTicket = null;
                        if (!string.IsNullOrWhiteSpace(ticketId))
                        {
                            existingTicket = await _ticketService.GetByIdAsync(ticketId);
                        }

                        // Nếu không tìm thấy ID, tạo ID mới
                        if (existingTicket == null)
                        {
                            ticketId = ObjectId.GenerateNewId().ToString();
                        }

                        if (existingTicket != null) // Vé đã tồn tại, cập nhật thông tin
                        {
                            existingTicket.TicketType = ticketTypeEnum.Value; // 🔹 Sửa lỗi TicketTy 
                            existingTicket.FromAddress = fromAddress;
                            existingTicket.ToAddress = toAddress;
                            existingTicket.FromDate = fromDate;
                            existingTicket.ToDate = toDate;
                            existingTicket.Quantity = quantity;
                            existingTicket.CustomerName = customerName;
                            existingTicket.CustomerPhone = customerPhone;
                            existingTicket.Status = ticketStatus.Value;

                            await _ticketService.UpdateAsync(ticketId, existingTicket);
                        }
                        else // Vé chưa tồn tại, tạo mới
                        {
                            var newTicket = new Ticket
                            {
                                Id = ticketId,
                                TicketType = ticketTypeEnum.Value, // 🔹 Sửa lỗi TicketType
                                FromAddress = fromAddress,
                                ToAddress = toAddress,
                                FromDate = fromDate,
                                ToDate = toDate,
                                Quantity = quantity,
                                CustomerName = customerName,
                                CustomerPhone = customerPhone,
                                Status = ticketStatus.Value
                            };

                            await _ticketService.CreateAsync(newTicket);
                        }
                    }
                }
            }
            return Ok(new { message = "Import danh sách vé thành công!" });
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

        [HttpGet("export-location")]
        public async Task<IActionResult> ExportTicketsToExcel([FromQuery] string? fromAddress, [FromQuery] string? toAddress)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Xây dựng bộ lọc dựa trên đầu vào
            var filter = Builders<Ticket>.Filter.Empty; // Mặc định lấy tất cả dữ liệu
            if (!string.IsNullOrEmpty(fromAddress) && !string.IsNullOrEmpty(toAddress))
            {
                    filter = Builders<Ticket>.Filter.And(
                    Builders<Ticket>.Filter.Eq(t => t.FromAddress, fromAddress),
                    Builders<Ticket>.Filter.Eq(t => t.ToAddress, toAddress)
                );
            }
            
            else if (!string.IsNullOrEmpty(fromAddress))
            {
                filter = Builders<Ticket>.Filter.Eq(t => t.FromAddress, fromAddress);
            }
            else if (!string.IsNullOrEmpty(toAddress))
            {
                filter = Builders<Ticket>.Filter.Eq(t => t.ToAddress, toAddress);
            }

            //var tickets = await _ticketService.Find(f => f.ToAddress == "Ca Mau" && f.FromAddress == "TPHCM"); // Lấy danh sách vé theo điều kiện lọc
            var tickets = await _ticketService.Find(filter);
            if (!tickets.Any())  return BadRequest("Không có dữ liệu để xuất.");
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

                // Ghi dữ liệu vào file Excel
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
                // lam dep ex
                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }


                // Lưu file vào MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tickets.xlsx");
            }
        }
        [HttpPost("export-by-filter")]
        public async Task<IActionResult> ExportTicketsToExcel([FromBody] FilterTicketDTO ticketDto)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Khởi tạo bộ lọc
            var filter = Builders<Ticket>.Filter.Empty; // Mặc định lấy tất cả dữ liệu

            var filters = new List<FilterDefinition<Ticket>>();
           


            if (!string.IsNullOrEmpty(ticketDto.TicketType))
            {
                filters.Add(Builders<Ticket>.Filter.Eq(t => t.TicketType.ToString(),  ticketDto.TicketType));
            }
            if (!string.IsNullOrEmpty(ticketDto.FromAddress))
            {
                filters.Add(Builders<Ticket>.Filter.Eq(t => t.FromAddress, ticketDto.FromAddress));
            }
            if (!string.IsNullOrEmpty(ticketDto.ToAddress))
            {
                filters.Add(Builders<Ticket>.Filter.Eq(t => t.ToAddress, ticketDto.ToAddress));
            }
            if (ticketDto.FromDate != null)
            {
                filters.Add(Builders<Ticket>.Filter.Gte(t => t.FromDate, ticketDto.FromDate));
            }
            if (ticketDto.ToDate != null)
            {
                filters.Add(Builders<Ticket>.Filter.Lte(t => t.ToDate, ticketDto.ToDate));
            }
            if (ticketDto.Quantity != null)
            {
                filters.Add(Builders<Ticket>.Filter.Eq(t => t.Quantity, ticketDto.Quantity));
            }
            if (!string.IsNullOrEmpty(ticketDto.CustomerName))
            {
                filters.Add(Builders<Ticket>.Filter.Eq(t => t.CustomerName, ticketDto.CustomerName));
            }
            if (!string.IsNullOrEmpty(ticketDto.CustomerPhone))
            {
                filters.Add(Builders<Ticket>.Filter.Eq(t => t.CustomerPhone, ticketDto.CustomerPhone));
            }
            if (!string.IsNullOrEmpty(ticketDto.Status))
            {
                filters.Add(Builders<Ticket>.Filter.Eq(t => t.Status, (TicketStatus)Enum.Parse(typeof(TicketStatus), ticketDto.Status)));
            }

            if (filters.Any())
            {
                filter = Builders<Ticket>.Filter.And(filters);
            }

            var tickets = await _ticketService.Find(filter);
            if (!tickets.Any()) return BadRequest("Không có dữ liệu để xuất.");

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

                // Ghi dữ liệu vào file Excel
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

                // Định dạng header
                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Lưu file vào MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tickets.xlsx");
            }
        }
        [HttpGet("export-sheet")]
        public async Task<IActionResult> ExportTicketsToExcel([FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            // Giới hạn các giá trị pageSize hợp lệ: 25, 50, 75, 100
            //int[] allowedPageSizes = { 25, 50, 75, 100 };
            //if (!allowedPageSizes.Contains(pageSize)) // content kiểm tra đúng nếu chứa value ngược lại trả false
            //{
            //    return BadRequest("PageSize không hợp lệ. Chọn 25, 50, 75 hoặc 100.");
            //}
            if (!(page > 0 && pageSize >0 ))
            {
                return BadRequest("Không hợp lê!");
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Lấy dữ liệu từ MongoDB
            var tickets = await _ticketService.Find(_ => true);

            // Phân trang
            var paginatedTickets = tickets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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

                // Ghi dữ liệu vào file Excel
                int row = 2;
                foreach (var ticket in paginatedTickets)
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
                stream.Position = 0; // đọc dữ liệu từ đầu 

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Tickets_Page{page}.xlsx");
            }
        }
        // 9 Require nhập liệu page và page size  sẽ hiển thị dưới dạng json nếu người dùng nhập liệu sai thì sẽ có xử lý ngoại lệ 
        [HttpGet("GetPageAndPageSize")]
        public async Task<IActionResult> GetAllTickets([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // 🔹 Kiểm tra giá trị hợp lệ
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Page and pageSize must be greater than 0"
                });
            }

            try
            {
                var tickets = await _ticketService.GetAllTicketsAsync(page, pageSize);
                return Ok(new { success = true, data = tickets });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while retrieving tickets",
                    error = ex.Message
                });
            }
        }
        // 10
        [HttpPost("import-update-status")]
        public async Task<IActionResult> ImportUpdateStatus([FromBody] List<UpdateTicketStatusDTO> updates)
        {
            if (updates == null || updates.Count == 0)
                return BadRequest(new { success = false, message = "Danh sách cập nhật trống!" });
            // kiểm tra ticketType có giá trị hợp lệ không
            var invalidTickets = updates.Where(u => !Enum.IsDefined(typeof(TicketType), u.Type)).ToList();
            // Kiểm tra status có hợp lệ
            var invalidStatuses = updates.Where(u => !Enum.IsDefined(typeof(TicketStatus), u.Status)).ToList();
            if (invalidTickets.Any() || invalidStatuses.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Danh sách cập nhật chứa dữ liệu không hợp lệ!",
                    invalidTickets,
                    invalidStatuses
                });
            }
            
            try
            {
                var updatedTickets = await _ticketService.UpdateTicketStatusAsync(updates);
                return Ok(new { success = true, message = "Cập nhật trạng thái thành công!", data = updatedTickets });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }


    }
}


