namespace CRUDOpperationMongoDB1.Models
{
    public class PagedResult<T>
    {
        // giúp tái sử dụng class chio mọi loại dữ liệu
        // Danh sách phần tử trong trang hiện tại
        public List<T> Items { get; set; }
        // Tống số phần tử trong toàn bộ dữ liệu
        public int TotalCount { get; set; }
        // Trang hiện tại
        public int Page {  get; set; }
        // Số phần tử mõi trang
        public int PageSize { get; set; }
        // Tổng số trang => tính từ TotalCount và PageSize
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        // có trang tiếp theo không
        public bool HasNextPage
        {
            get
            {
                return Page < TotalPages;
            }
        }
        // Có trang trước không
        public bool HasPreviousPage
        {
            get
            {
                return Page > 1;
            }
        }
    }
}
