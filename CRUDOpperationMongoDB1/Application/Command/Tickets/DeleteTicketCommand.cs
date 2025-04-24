using MediatR;
namespace CRUDOpperationMongoDB1.Application.Command.Tickets
{
    public class DeleteTicketCommand : IRequest<bool>
    {
        public string Id {  get; set; }
    }
}
