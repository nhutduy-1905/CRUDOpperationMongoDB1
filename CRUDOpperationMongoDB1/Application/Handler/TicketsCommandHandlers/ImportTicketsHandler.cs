using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OfficeOpenXml;

namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    public class ImportTicketsHandler : IRequestHandler<ImportTicketsCommand, IActionResult>
    {
        private readonly ITicketRepository _tickeRepository;

        public ImportTicketsHandler(ITicketRepository tickeRepository)
        {
            _tickeRepository = tickeRepository;
        }

        public async Task<IActionResult> Handle(ImportTicketsCommand request, CancellationToken cancellationToken)
        {
            // kiểm tra file có null hoặc rỗng
            if (request.File == null || request.File.Length == 0)
                return new BadRequestObjectResult("Vui long chon file Excel.");
            // Dùng thư viện excel, đọc nội dung MemoryStream, tạo object ExcelPackage
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null || worksheet.Dimension == null)
                return new BadRequestObjectResult("File Excel không có dữ liệu hoặc sai định dạng.");

            int rowCount = worksheet.Dimension.Rows;
            var allRows = new List<(int Row, string TicketId, string TicketType, string FromAddress, string ToAddress, string FromDate, string ToDate, int Quantity, string CustomerName, string CustomerPhone, string Status, string Result)>();
            // đọc dữ liệu từng dòng, đọc dữ liệu và lưu vào biến
            for (int row = 2; row <= rowCount; row++)
            {
                var ticketId = worksheet.Cells[row, 1].Text.Trim();
                var ticketType = worksheet.Cells[row, 2].Text.Trim();
                var fromAddress = worksheet.Cells[row, 3].Text.Trim();
                var toAddress = worksheet.Cells[row, 4].Text.Trim();
                var fromDated = worksheet.Cells[row, 5].Text.Trim();
                var toDated = worksheet.Cells[row, 6].Text.Trim();
                var quantityT = worksheet.Cells[row, 7].Text.Trim();
                var customerName = worksheet.Cells[row, 8].Text.Trim();
                var customerPhone = worksheet.Cells[row, 9].Text.Trim();
                var ticketStatus = worksheet.Cells[row, 10].Text.Trim();

                // Xác thực dữ liệu từng dòng --> nếu có lỗi hiển thị Result , không có lỗi thì tiếp tục cập nhật 
                var errorBuilder = new List<string>();
                if (string.IsNullOrWhiteSpace(ticketId)) errorBuilder.Add("TicketId bị rỗng");
                TicketType ticketTypeEnum = TicketType.MotChieu;
                if (string.IsNullOrWhiteSpace(ticketType) || !TryParseTicketType(ticketType, out ticketTypeEnum))
                {
                    errorBuilder.Add($"TicketType không hợp lệ ({ticketType})");
                }
                TicketStatus status = TicketStatus.Active;
                if (string.IsNullOrWhiteSpace(ticketStatus) || !TryParseTicketStatus(ticketStatus, out status))
                {
                    errorBuilder.Add($"TicketStatus không hợp lệ ({ticketStatus})");
                }

                if (!DateTime.TryParse(fromDated, out var fromDate)) errorBuilder.Add($"FromDate không hợp lệ ({fromDated})");
                if (!DateTime.TryParse(toDated, out var toDate)) errorBuilder.Add($"ToDate không hợp lệ ({toDated})");
                if (!int.TryParse(quantityT, out var quantity))
                    errorBuilder.Add($"Quantity không hợp lệ ({quantityT})");

                string result = errorBuilder.Any() ? string.Join("; ", errorBuilder) : "Success";
                allRows.Add((row, ticketId, ticketTypeEnum.ToString(), fromAddress, toAddress, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"), quantity, customerName, customerPhone, status.ToString(), result));

                if (!errorBuilder.Any())
                {
                    // nếu vé đã tồn tại dựa vào ticketId thì cập nhật các trường dữ liệu
                    var existingTicket = await _tickeRepository.GetTicketByIdAsync(ticketId);
                    if (existingTicket != null)
                    {
                        existingTicket.TicketType = ticketTypeEnum;
                        existingTicket.FromAddress = fromAddress;
                        existingTicket.ToAddress = toAddress;
                        existingTicket.FromDate = fromDate;
                        existingTicket.ToDate = toDate;
                        existingTicket.Quantity = quantity;
                        existingTicket.CustomerName = customerName;
                        existingTicket.CustomerPhone = customerPhone;
                        existingTicket.Status = status;

                        await _tickeRepository.UpdateTicketAsync(existingTicket.Id, existingTicket);
                    }
                }
            }

            // Tạo file excel mới, dòng đầu là tiêu đề
            using var resultPackage = new ExcelPackage();
            var resultSheet = resultPackage.Workbook.Worksheets.Add("Result");
            resultSheet.Cells[1, 1].Value = "Dòng";
            resultSheet.Cells[1, 2].Value = "TicketId";
            resultSheet.Cells[1, 3].Value = "TicketType";
            resultSheet.Cells[1, 4].Value = "FromAddress";
            resultSheet.Cells[1, 5].Value = "ToAddress";
            resultSheet.Cells[1, 6].Value = "FromDate";
            resultSheet.Cells[1, 7].Value = "ToDate";
            resultSheet.Cells[1, 8].Value = "Quantity";
            resultSheet.Cells[1, 9].Value = "CustomerName";
            resultSheet.Cells[1, 10].Value = "CustomerPhone";
            resultSheet.Cells[1, 11].Value = "Status";
            resultSheet.Cells[1, 12].Value = "Result";

            for (int i = 0; i < allRows.Count; i++)
            {
                resultSheet.Cells[i + 2, 1].Value = allRows[i].Row;
                resultSheet.Cells[i + 2, 2].Value = allRows[i].TicketId;
                resultSheet.Cells[i + 2, 3].Value = allRows[i].TicketType;
                resultSheet.Cells[i + 2, 4].Value = allRows[i].FromAddress;
                resultSheet.Cells[i + 2, 5].Value = allRows[i].ToAddress;
                resultSheet.Cells[i + 2, 6].Value = allRows[i].FromDate;
                resultSheet.Cells[i + 2, 7].Value = allRows[i].ToDate;
                resultSheet.Cells[i + 2, 8].Value = allRows[i].Quantity;
                resultSheet.Cells[i + 2, 9].Value = allRows[i].CustomerName;
                resultSheet.Cells[i + 2, 10].Value = allRows[i].CustomerPhone;
                resultSheet.Cells[i + 2, 11].Value = allRows[i].Status;
                resultSheet.Cells[i + 2, 12].Value = allRows[i].Result;
            }

            var resultStream = new MemoryStream();
            resultPackage.SaveAs(resultStream);
            resultStream.Position = 0;

            var fileContent = resultStream.ToArray();
            var fileName = $"Import_Result_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return new FileContentResult(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
        }
        // chuyển chuỗi thành enum TicketType --> nếu không hợp lệ trả về false
        private bool TryParseTicketType(string value, out TicketType ticketType)
        {
            if (Enum.TryParse<TicketType>(value, true, out ticketType))
            {
   
                return true;
            }
            ticketType = TicketType.MotChieu; // Default value
            return false;
        }

        private bool TryParseTicketStatus(string value, out TicketStatus status)
        {
            if (Enum.TryParse<TicketStatus>(value, true, out status))
            {
                return true;
            }
            status = TicketStatus.Active; // Default value
            return false;
        }
    }
}