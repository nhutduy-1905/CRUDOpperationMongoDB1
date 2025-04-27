using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Shared;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    // Command dung gui yeu cau cap nhat nhieu ttang thai ve
    public class ImportUpdateStatusCommand : IRequest<Result<string>>
    {
        //Danh sach DTO chua thong tin ve va trang thai moi
        public List<UpdateTicketStatusDTO> Updates { get; set; }
    }
   
}
