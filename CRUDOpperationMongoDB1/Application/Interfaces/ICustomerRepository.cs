using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Models;
using CRUDOpperationMongoDB1.Shared;
namespace CRUDOpperationMongoDB1.Application.Interfaces
{
    public interface ICustomerRepository
    {
        // Them mới một khách hàng
        Task<Result<Customer>> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
        // Tìm khách hàng theo id
        Task<Result<Customer>> GetCustomerByIdAsync(string customerId, CancellationToken cancellationToken = default);
        // Tìm khách hàng theo email
        Task<Result<Customer>> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken = default);
        // Câp nhật toàn bộ thông tin khách hàng
        Task<Result> UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
        // xóa khách hàng theo id
        Task<Result> DeleteCustomerAsync(string customerId, CancellationToken cancellation = default);
        // Lấy danh sách khách hàng có phân trang
        Task<PagedResult<Customer>> ListCustomersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
