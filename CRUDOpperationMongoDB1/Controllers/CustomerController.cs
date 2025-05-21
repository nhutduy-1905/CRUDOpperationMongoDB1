using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Application.Handler.QueryHandlers;
using MediatR;
using CRUDOpperationMongoDB1.Application.Queries.Customers;
using Microsoft.AspNetCore.Mvc;
using CRUDOpperationMongoDB1.Application.Queries.CustomerQueries;
using CRUDOpperationMongoDB1.Shared;
namespace CRUDOpperationMongoDB1.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var customerId = await _mediator.Send(command);
            return Ok (new { CustomerId = customerId });
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
           var customer = await _mediator.Send(new GetCustomerByIdQuery(customerId));
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }
        [HttpGet("get-by-email/{email}")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            try
            {
                var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));
                if (customer == null)
                    return NotFound(new { success = false, message = "Không tìm thấy khách hàng!" });
                return Ok(new { success = true, data = customer });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (MongoDB.Bson.BsonSerializationException ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi ánh xạ dữ liệu MongoDB: " + ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new ListCustomersQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] UpdateCustomerCommand command)
        {
            if (id != command.CustomerId) return BadRequest(new { success = false, message = "ID không hợp lệ" });
            var result = await _mediator.Send(command);
            return Ok(result);
               
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var result = await _mediator.Send(new DeleteCustomerCommand { CustomerId = id });
            return result.IsSuccess ? Ok("Xóa thành công") : NotFound(result.ErrorMessage);
        }
    }
}
