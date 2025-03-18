using TicketAPI.Models;

namespace TicketAPI.DTOs
{
    public class CreateTicketDTO
    {
        public String TicketType { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; } // Thêm trạng thái vào DTO
    }

    public class FilterTicketDTO
    {
        public string? TicketType { get; set; }
        public string? FromAddress { get; set; }
        public string? ToAddress { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Quantity { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? Status { get; set; }

        // Thêm phân trang
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    // thêm 
    public class UpdateTicketStatusDTO
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    } 
}

