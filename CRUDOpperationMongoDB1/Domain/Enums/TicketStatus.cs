using CRUDOpperationMongoDB1.Application.DTO;
using System.ComponentModel.DataAnnotations;

namespace CRUDOpperationMongoDB1.Domain.Enums;

public enum TicketStatus
{
   
    [Display(Name = "Đang hoạt động")]
    Active,

    [Display(Name = "Không hoạt động")]
    Inactive,

    [Display(Name = "Đã xóa")]
    Deleted
}
