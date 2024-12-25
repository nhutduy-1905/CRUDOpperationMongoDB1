using CRUDOpperationMongoDB1.Models;
using CRUDOpperationMongoDB1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDOpperationMongoDB1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<List<Items>> Get() =>
            await _itemService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Items>> Get(string id)
        {
            var item = await _itemService.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Items newItem)
        {
            await _itemService.CreateAsync(newItem);
            return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Items updatedItem)
        {
            var item = await _itemService.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            updatedItem.Id = item.Id;

            await _itemService.UpdateAsync(id, updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _itemService.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            await _itemService.RemoveAsync(id);

            return NoContent();
        }
    }

}
