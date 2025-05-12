using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace CRUDOpperationMongoDB1.Domain.Enums;


public enum TicketType
{
    [Display(Name = "Khứ hồi")]
    KhuHoi,

    [Display(Name = "Một chiều")]
    MotChieu
}
