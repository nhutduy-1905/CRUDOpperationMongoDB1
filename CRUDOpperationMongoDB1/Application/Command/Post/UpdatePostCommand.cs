using MediatR;
// dùng gửi command (IRequest) đến Handler xử lý logic
namespace CRUDOpperationMongoDB1.Application.Command.Post
{
    // Class: yêu cầu cập nhật bài post
    // trả về kiểu bool --> true, false
    public class UpdatePostCommand :IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content {  get; set; }
    }
}
