using HelpDesk.Domain.Enums;

namespace HelpDesk.Application.DTOs;

public class CrearTicketDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public TicketPrioridad Prioridad { get; set; }
    public int CategoriaId { get; set; }
}
