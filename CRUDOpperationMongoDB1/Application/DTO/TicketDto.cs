using CRUDOpperationMongoDB1.Domain.Enums;

namespace CRUDOpperationMongoDB1.Application.DTO;

public class TicketDto
{
    public string Id {  get; set; }
    public string TicketType { get;  set; }
    public string FromAddress {  get; set; }
    public string ToAddress { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; } 
    public int Quantity { get; set; }
    public string CustomerName {  get; set; }
    public string CustomerPhone { get;  set; }
    public string Status {  get; set; }
  
}
