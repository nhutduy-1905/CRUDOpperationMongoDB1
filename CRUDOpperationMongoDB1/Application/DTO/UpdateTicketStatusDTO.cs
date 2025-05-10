using CRUDOpperationMongoDB1.Domain.Enums;

namespace CRUDOpperationMongoDB1.Application.DTO
{
    public class UpdateTicketStatusDTO
    {
        public string TicketId { get; set; } // Ma ve
        public TicketType Type { get; set; } // loai ve
        public TicketStatus Status { get; set; } // trang thai moi
    }
}
