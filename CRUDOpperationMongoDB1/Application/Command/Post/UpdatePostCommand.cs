using MediatR;

namespace CRUDOpperationMongoDB1.Application.Command.Post
{
    public class UpdatePostCommand :IRequest<bool>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content {  get; set; }
    }
}
