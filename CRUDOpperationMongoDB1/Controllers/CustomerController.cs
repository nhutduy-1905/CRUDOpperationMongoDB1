using CRUDOpperationMongoDB1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TicketAPI.Service;
using TicketAPI.Services;

[Route("api/customers")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly TicketService _ticketService;
    public CustomerController(CustomerService customerService, TicketService ticketService)
    {
        _customerService = customerService;
        _ticketService = ticketService;
    }
    [HttpGet("get-by-email/{email}")]
    public async Task<IActionResult> GetCustormerByEmail(string email)
    {
        var customer = await _customerService.GetCustomerByEmailAsync(email);
        if (customer == null)
            return NotFound(new { success = false, message = "Không tìm thấy khách hàng!" });
        return Ok(new { success = true, data = customer });
    }
    [HttpGet("get-by-customer/{customerId}")]
    public async Task<IActionResult> GetTicketsByCustomerId(string customerId, int page = 1, int pageSize = 10)
    {
        if (string.IsNullOrEmpty(customerId))
            return BadRequest(new { success = false, message = "CustomerId không hợp lệ!" });
        if (page < 1 || pageSize < 1)
        {
            return BadRequest(new { success = false, message = "Page và  PageSize phải lớn hơn 1!" });
        }
        var tickets = await _ticketService.GetTicketsByCustomerIdAsync(customerId, page, pageSize);
        return Ok(new { success = true, data = tickets });
    }
    // Tạo khách hàng mới
    [HttpPost("create")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDTO customerDTO)
    {
        if (string.IsNullOrEmpty(customerDTO.CustomerName) ||
            string.IsNullOrEmpty(customerDTO.CustomerPhone) ||
            string.IsNullOrEmpty(customerDTO.Email))
        {
            return BadRequest(new { success = false, message = "Thông tin khách hàng không hợp lệ!" });
        }
        // kiểm tra email
        if (!IsValidEmail(customerDTO.Email))
        {
            return BadRequest(new { success = false, message = "Email không hợp lệ!" });
        }
        var customer = CustomerMapping.ToEntity(customerDTO);
        var customerResult = await _customerService.CreateCustomerAsync(customer);
        return Ok(new { success = true, message = "Tạo tài khoản khách hàng thành công!", data = customerResult });
    }
    private bool IsValidEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; 
        return Regex.IsMatch(email, emailPattern);
    }
    [HttpPost("test")]
    public async Task<IActionResult> Test(string ticketCode)
    {
        var success = IsValidTicketCode(ticketCode);
        if (success)
            return Ok(new { success = true, });
        return BadRequest(new { success = false, });
       
    }
    private bool IsValidTicketCode(string ticketCode)
    {
        string ticketCodes = @"^[A-Z0-9]+$";
        return Regex.IsMatch(ticketCode, ticketCodes);
    }
}