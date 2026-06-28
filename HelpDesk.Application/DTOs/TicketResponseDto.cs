using HelpDesk.Domain.Enums;

namespace HelpDesk.Application.DTOs;

public class TicketResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public TicketPrioridad Prioridad { get; set; }
    public TicketEstado Estado { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
