using HelpDesk.Domain.Enums;

namespace HelpDesk.Domain.Entities;

public class Ticket
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public TicketPrioridad Prioridad { get; set; }
    public TicketEstado Estado { get; set; } = TicketEstado.Abierto;
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    public int CreadorId { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
