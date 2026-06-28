namespace HelpDesk.Domain
{
    public class Ticket
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}