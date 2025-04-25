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
        public async Task<IActionResult> Handle(ImportTicketsCommand request,  CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                return new BadRequestObjectResult("Vui long chon file Excel.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var stream = new MemoryStream(); ;
            await request.File.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null || worksheet.Dimension == null)
                return new BadRequestObjectResult("File excel khing co du lieu hoac sai dinh dang.");

            int rowCount = worksheet.Dimension.Rows;
            var error = new List<string>();

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

                if (!Enum.TryParse<TicketType>(ticketType, true, out var ticketTypeEnum))
                    error.Add($"Dong {row}: TicketType khong hop le ({ticketType})");
                if(!Enum.TryParse<TicketStatus>(ticketStatus, true, out var status))
                    error.Add($"Dong {row}: TicketStatus khong hop le ({ticketStatus})");
                if (!DateTime.TryParse(fromDated, out var fromDate))
                    error.Add($"Dong {row}: FromDate khong hop le ({fromDated})");
                if (!DateTime.TryParse(toDated, out var toDate))
                    error.Add($"Donh {row}: FromDate khong hop le ({toDate}");
                if (!int.TryParse(quantityT, out var quantity))
                    error.Add($"Dong {row}: Quantity khong hop le ({quantity})");

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
            if (error.Any())
                return new BadRequestObjectResult(new { message = "Import that bai", error });
            return new OkObjectResult(new { message = "Import danh sach ve thanh cong." });
        }
    }
}
