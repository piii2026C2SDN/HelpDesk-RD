using HelpDesk.Application.DTOs;
using HelpDesk.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDesk.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearTicketDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        var creadorId = int.Parse(userIdClaim);

        try
        {
            var ticket = await _ticketService.CrearTicketAsync(dto, creadorId);
            return CreatedAtAction(nameof(Listar), new { id = ticket.Id }, ticket);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var tickets = await _ticketService.ListarTicketsAsync();
        return Ok(tickets);
    }
}
