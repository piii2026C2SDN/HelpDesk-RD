using HelpDesk.Application.DTOs;

namespace HelpDesk.Application.Interfaces;

public interface ITicketService
{
    Task<TicketResponseDto> CrearTicketAsync(CrearTicketDto dto, int creadorId);
    Task<IEnumerable<TicketResponseDto>> ListarTicketsAsync();
}
