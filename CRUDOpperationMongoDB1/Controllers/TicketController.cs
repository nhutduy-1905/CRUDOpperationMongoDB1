using CRUDOpperationMongoDB1.Application.Command.Tickets;
using CRUDOpperationMongoDB1.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Queries.TicketsQueries;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // get api/ ticket
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllTicketsQuery());
        return Ok(result);
    }
    // get api/ tickets/{id}
    [HttpGet("Get")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetTicketByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }
    // new
    [HttpGet("paged/export-excel")]
    public async Task<IActionResult> ExportGetPagedExcel([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetPagedTicketsQuery(page, pageSize));

        using (var package = new ExcelPackage())
        {
            var ws = package.Workbook.Worksheets.Add("Tickets");

            // Header
            ws.Cells[1, 1].Value = "Id";
            ws.Cells[1, 2].Value = "TicketType";
            ws.Cells[1, 3].Value = "FromAddress";
            ws.Cells[1, 4].Value = "ToAddress";
            ws.Cells[1, 5].Value = "FromDate";
            ws.Cells[1, 6].Value = "ToDate";
            ws.Cells[1, 7].Value = "Quantity";
            ws.Cells[1, 8].Value = "CustomerName";
            ws.Cells[1, 9].Value = "CustomerPhone";
            ws.Cells[1, 10].Value = "Status";

            // Dữ liệu từng dòng
            for (int i = 0; i < result.Items.Count; i++)
            {
                var ticket = result.Items[i];
                ws.Cells[i + 2, 1].Value = ticket.Id;
                ws.Cells[i + 2, 2].Value = ticket.TicketType.ToString();
                ws.Cells[i + 2, 3].Value = ticket.FromAddress;
                ws.Cells[i + 2, 4].Value = ticket.ToAddress;
                ws.Cells[i + 2, 5].Value = ticket.FromDate.ToString("yyyy-MM-dd");
                ws.Cells[i + 2, 6].Value = ticket.ToDate.ToString("yyyy-MM-dd");
                ws.Cells[i + 2, 7].Value = ticket.Quantity;
                ws.Cells[i + 2, 8].Value = ticket.CustomerName;
                ws.Cells[i + 2, 9].Value = ticket.CustomerPhone;
                ws.Cells[i + 2, 10].Value = ticket.Status.ToString();
            }

            // Chuyển đổi nội dung Excel thành byte array
            var fileBytes = package.GetAsByteArray();

            // Trả về file Excel cho người dùng, yêu cầu tải về và mở trực tiếp
            var fileName = $"tickets_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            Response.Headers.Append("Content-Disposition", "attachment; filename=" + fileName); // yêu cầu tải về
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }



    // new
    [HttpGet("export-location")]
    public async Task<IActionResult> ExportLocation([FromQuery] string? fromAddress, [FromQuery] string? toAddress)
    {
        try
        {
            var result = await _mediator.Send(new ExportTicketsByLocationQuery { FromAddress = fromAddress, ToAddress = toAddress });
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tickets.xlsx");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    // new
    [HttpGet("export-sheet")]
    public async Task<IActionResult> ExportPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        if (page <= 0 || pageSize <= 0) return BadRequest("Khong hop le.");
        var result = await _mediator.Send(new ExportTicketsPaginatedQuery { Page = page, PageSize = pageSize });
        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Tickets_Page{page}.xlsx");
    }




    //  api/tickets/search
    [HttpPost("search")]
    public async Task<IActionResult> SearchTickets([FromBody] SearchTicketsQuery query)
    {
        Console.WriteLine($"Controller - FromAddress: {query.FromAddress}");
        Console.WriteLine($"Controller - ToAddress: {query.ToAddress}");
        Console.WriteLine($"Controller - TicketType: {query.TicketType}");
        Console.WriteLine($"Controller - Status: {query.Status}");
        Console.WriteLine($"Controller - FromDate: {query.FromDate}");
        Console.WriteLine($"Controller - ToDate: {query.ToDate}");

        var result = await _mediator.Send(query); // Gọi SearchTicketsQueryHandler
        Console.WriteLine($"Controller - Found {result.Count} tickets");
        return Ok(result);
    }
    // ENDPOINT DE TAO VE
    // api/ tickets
    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { TicketId = id });
    }
    // Import tickets from Excel
    [HttpPost("import")]
    public async Task<IActionResult> ImportTickets(IFormFile file)
    {
        var result = await _mediator.Send(new ImportTicketsCommand { File = file });
        return result;
    }
    // Export tickets to Excel by filter
    [HttpPost("export-by-filter")]
    public async Task<IActionResult> ExportTickets([FromBody] TicketDto filter)
    {
        var result = await _mediator.Send(new ExportTicketsQuery { Filter = filter });
        return result;
    }
    // API cap nhat trang thai nhieu ve
    [HttpPost("import-update-status")]
    public async Task<IActionResult> ImportUpdateStatus([FromBody] List<UpdateTicketStatusDTO> updates)
    {
        var command = new ImportUpdateStatusCommand { Updates = updates };
        var result = await _mediator.Send(command);
        if (result == null || result.IsSuccess == false) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTicketCommand command)
    {
        if (id != command.Id) return BadRequest("Is mismath");
        var result = await _mediator.Send(command);
        if (result.Id == null)
        {
            return Ok(new { success = false });
        }
        return Ok(new {success = true, result = result});
    }

    
    //DELETE: api/tickets/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var command = new DeleteTicketCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result) 
            return NotFound(new {message = "Ticket not found or already deleted."});
        return Ok(new {message = $"Ticket with id {id} has been successfully deleted."} );
        
    }
    // api huy mot ve
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> CancelTicket(string id)
    {
        var command = new CancelTicketCommand(id);
        var result = await _mediator.Send(command);
        if (result == null || result.IsSuccess == false) return NotFound(result);
        return Ok(result);
    }
}

