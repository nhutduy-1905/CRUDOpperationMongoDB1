using CRUDOpperationMongoDB1.Domain.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;

public static class ExcelHelper 
{ 
    public static ExcelPackage GenerateExcel(List<Ticket> tickets)
    {
        var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Tickets");

        ws.Cells[1, 1].Value = "ID";
        ws.Cells[1, 2].Value = "Loai Ve";
        ws.Cells[1, 3].Value = "Diem Di";
        ws.Cells[1, 4].Value = "Diem Den";
        ws.Cells[1, 5].Value = "Ngay Di";
        ws.Cells[1, 6].Value = "Ngay Den";
        ws.Cells[1, 7].Value = "So Luong";
        ws.Cells[1, 8].Value = "Ten Khach Hang";
        ws.Cells[1, 9].Value = "SDT Khach Hang";
        ws.Cells[1, 10].Value = "Trang Thai";

        int row = 2;
        foreach (var t in tickets)
        {
            ws.Cells[row, 1].Value = t.Id;
            ws.Cells[row, 2].Value = t.TicketType;
            ws.Cells[row, 3].Value = t.FromAddress;
            ws.Cells[row, 4].Value = t.ToAddress;
            ws.Cells[row, 5].Value = t.FromDate.ToString("yyyy-MM-dd HH:mm");
            ws.Cells[row, 6].Value = t.ToDate.ToString("yyyy-MM-dd HH:mm");
            ws.Cells[row, 7].Value = t.Quantity;
            ws.Cells[row, 8].Value = t.CustomerName;
            ws.Cells[row, 9].Value = t.CustomerPhone;
            ws.Cells[row, 10].Value = t.Status;
            row++;
        }
        using (var range = ws.Cells[1, 1, 1, 10])
        {
            range.Style.Font.Bold = true;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
      

        return package;

    }
}

