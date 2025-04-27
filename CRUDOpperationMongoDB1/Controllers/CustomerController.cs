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
        [HttpPost("{CreateCustomer}")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var customerId = await _mediator.Send(command);
            return Ok (new { CustomerId = customerId });
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
           var customer = await _mediator.Send(new GetCustomerByIdQuery { CustomerId = customerId });
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }
    }
}
