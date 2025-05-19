using System.Diagnostics;

namespace CRUDOpperationMongoDB1.Models
{
    public class ErrorResponse
    {
        // Luôn false với reponse lỗi, giúp client dễ kiểm tra
        public bool IsSuccess => false;
        public int Status { get; set; }
        // Mã lỗi đồng bộ, giúp fontend dễ phân biệt tình huống lỗi
        public string Code { get; set; }
        // Thông điệp gưi về client
        public string Message {  get; set; }
        // Thông điêph thân thiện cho người dùng cuối, có thể đã được localization
        public string UserMessage { get; set; }
        // Thông tin chi tiết kỹ thuật , stack trace hoặc nguyên nhân gốc
        // chỉ nên dùng trong môi trường development hoặc để log
        public string Details { get; set; }
        // Thơi điểm server tạo ra lỗi (UTC), giúp trace log dễ hơn
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        // Liên kết với request với log
        public string TraceId { get; set; } = Activity.Current?.Id;
        // Danh sách lôi chi tiết ở cấp field khi validation không thành công
        public List<FieldError> Errors { get; set; } = new();
        // Chi tiết trong từng Field
        public class FieldError
        {
            public string Fileld { get; set; }
            public string Error { get; set; }
        }

    }
}
