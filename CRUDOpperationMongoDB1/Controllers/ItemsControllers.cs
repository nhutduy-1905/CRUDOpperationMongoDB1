using CRUDOpperationMongoDB1.Models;
using CRUDOpperationMongoDB1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDOpperationMongoDB1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private static List<Ticket> tickets = new List<Ticket>();
        private static int idCounter = 1;

        [HttpPost("createTicket")]
        public IActionResult CreateTicket([FromBody] TicketDTO ticketDTO)
        {
            if (ticketDTO == null) return BadRequest("Invalid ticket data.");

            // Mapping từ DTO sang Entity
            var ticket = ticketDTO.ToTicket();
            ticket.Id = idCounter++; // Auto-generate Id

            // Lưu vào danh sách hoặc cơ sở dữ liệu
            tickets.Add(ticket);

            return Ok(new { Message = "Ticket created successfully.", TicketId = ticket.Id });
        }
    }
}

//    [ApiController]
//    [Route("api/[controller]")]
//    public class ItemsController : ControllerBase
//    {
//        private readonly ItemService _itemService;

//        public ItemsController(ItemService itemService)
//        {
//            _itemService = itemService;
//        }
//        [HttpGet]
//        public async Task<List<ItemDTO>> Get() =>
//            await _itemService.GetAsync();

//        [HttpGet("{id}")]
//        public async Task<ActionResult<ItemDTO>> Get(string id)
//        {
//            var item = await _itemService.GetAsync(id);

//            if (item is null)
//            {
//                return NotFound();
//            }

//            return item;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Post(CreateItemDTO dto)
//        {
//            var newItem = await _itemService.CreateAsync(dto);
//            return Ok(newItem);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(string id, Items updatedItem)
//        {
//            var item = await _itemService.GetAsync(id);

//            if (item is null)
//            {
//                return NotFound();
//            }

//            updatedItem.Id = item.Id;

//            await _itemService.UpdateAsync(id, updatedItem);

//            return Ok();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var item = await _itemService.GetAsync(id);

//            if (item is null)
//            {
//                return NotFound();
//            }

//            await _itemService.RemoveAsync(id);

//            return Ok();
//        }

//    }

//}
