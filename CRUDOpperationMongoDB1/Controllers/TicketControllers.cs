using Microsoft.AspNetCore.Mvc;
using TicketAPI.Models;
using TicketAPI.Services;
using TicketAPI.DTOs;
using TicketAPI.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketAPI.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDTO ticketDto)
        {
            if (ticketDto == null || string.IsNullOrEmpty(ticketDto.CustomerPhone))
            {
                return BadRequest(new { error = "Invalid ticket data" });
            }

            var ticket = TicketMapper.ToEntity(ticketDto);
            await _ticketService.CreateAsync(ticket);

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, TicketMapper.ToDTO(ticket));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(string id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null) return NotFound(new { error = "Ticket not found" });

            return Ok(TicketMapper.ToDTO(ticket));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketService.GetAsync();
            var ticketDTOs = tickets.ConvertAll(ticket => TicketMapper.ToDTO(ticket));

            return Ok(ticketDTOs);
        }
    }
}
