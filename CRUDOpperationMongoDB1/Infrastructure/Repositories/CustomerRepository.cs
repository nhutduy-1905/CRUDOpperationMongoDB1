using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Data;
using CRUDOpperationMongoDB1.Domain.Entities;
using MongoDB.Driver;
using CRUDOpperationMongoDB1.Shared;
using CRUDOpperationMongoDB1.Models;

namespace CRUDOpperationMongoDB1.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;
        private readonly ILogger<CustomerRepository> _logger;
        public CustomerRepository(IMongoDatabase context, ILogger<CustomerRepository> logger)
        {
            _customers = context.GetCollection<Customer>("Customers");
            _logger = logger;

            // Tao chỉ mục duy nhất cho trường email
            var emailIndex = Builders<Customer>.IndexKeys.Ascending(c => c.Email);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<Customer>(emailIndex, indexOptions);
            _customers.Indexes.CreateOne(indexModel);
            _logger.LogInformation("Tạo chỉ mục duy nhất trên trường Email của Customers");
        }
        // Tạo custoemer
        public async Task<Result<Customer>> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            try
            {
                await _customers.InsertOneAsync(customer, cancellationToken: cancellationToken);
                _logger.LogInformation("Thêm customer thành công với ID {CustomerId}", customer.CustomerId);
                return Result<Customer>.Success(customer);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                _logger.LogWarning("Email bị trùng: {Email}", customer.Email);
                return Result<Customer>.Failure("Email đã tồn tại");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm customer");
                return Result<Customer>.Failure("Lỗi máy chủ nội bộ");
            }
        }
        // Tim customer theo Id
        public async Task<Result<Customer>> GetCustomerByIdAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.CustomerId, customerId);
            var customer = await _customers.Find(filter).FirstOrDefaultAsync(cancellationToken);
            if (customer == null)
            {
                return Result<Customer>.Failure("Không tìm thấy khách hàng");
            }
            return Result<Customer>.Success(customer);  
                
        }
        // xóa customer theo ìd
        public async Task<Result> DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var result = await _customers.DeleteOneAsync( c => c.CustomerId == customerId, cancellationToken);
            if (result.DeletedCount == 0) return Result.Failure("Không tìm thấy khách hàng để xóa");
            return Result.Success();
        }
        // tìm theo email
        public async Task<Result<Customer>> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Email, email);
            var customer = await _customers.Find(c => c.Email == email).FirstOrDefaultAsync(cancellationToken);
            if (customer == null)
                return Result<Customer>.Failure("Không tìm thấy khách hàng");
            return Result<Customer>.Success(customer);
            
        }
        // update customer
        public async Task<Result> UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            var result = await _customers.ReplaceOneAsync(c => c.CustomerId == customer.CustomerId, customer, cancellationToken: cancellationToken);
            if (result.MatchedCount == 0) return Result.Failure("Không tìm thấy khách hàng để cập nhật");
            return Result.Success();
        }
        // phân trang customer
        public async Task<PagedResult<Customer>> ListCustomersAsync(int page,  int pageSize, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Customer>.Filter.Empty;
            var total = (int)await _customers.CountDocumentsAsync(filter, cancellationToken :cancellationToken);
            var items = await _customers.Find(filter).Skip((page -1) * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
            return new PagedResult<Customer>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize

            };
        }
    }
}
