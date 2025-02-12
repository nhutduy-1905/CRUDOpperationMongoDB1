using CRUDOpperationMongoDB1.Models;
using CRUDOpperationMongoDB1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDOpperationMongoDB1.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
   

    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Lấy danh sách tất cả vé
        [HttpGet]
        public async Task<List<TicketDTO>> Get() =>
            await _ticketService.GetAllTicketsAsync();

        // Lấy vé theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> Get(string id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            return ticket;
        }

        // Tạo vé mới
        [HttpPost]
        public async Task<IActionResult> Post(CreateTicketDTO dto)
        {
            var newTicket = await _ticketService.CreateTicketAsync(dto);
            return Ok(newTicket);
        }

        // Cập nhật vé
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Ticket updatedTicket)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            updatedTicket.Id = ticket.Id;

            await _ticketService.UpdateTicketAsync(id, updatedTicket);

            return Ok();
        }

        // Xóa vé theo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            await _ticketService.RemoveTicketAsync(id);

            return Ok();
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
