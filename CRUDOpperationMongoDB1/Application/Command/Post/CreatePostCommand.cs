using MediatR;
// cho phép gửi các command hoặc query tới handler xử lý
namespace CRUDOpperationMongoDB1.Application.Command.Post
{
    // class yêu cầu tạo một bài viết mới
    // Guid là id của bài viết khi tạo
    public class CreatePostCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
