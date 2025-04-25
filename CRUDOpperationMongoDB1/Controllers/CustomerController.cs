using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Application.Handler.QueryHandlers;
using MediatR;
using CRUDOpperationMongoDB1.Application.Queries.Customers;
using Microsoft.AspNetCore.Mvc;

namespace CRUDOpperationMongoDB1.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
            var query = new GetCustomerByIdQuery(customerId);
            var customer = await _mediator.Send(query);
            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound();
        }
    }
}
