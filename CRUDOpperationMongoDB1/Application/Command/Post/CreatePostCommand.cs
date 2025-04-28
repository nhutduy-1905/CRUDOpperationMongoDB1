using MediatR;

namespace CRUDOpperationMongoDB1.Application.Command.Post
{
    public class CreatePostCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
