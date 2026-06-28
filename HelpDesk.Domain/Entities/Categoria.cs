namespace HelpDesk.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
