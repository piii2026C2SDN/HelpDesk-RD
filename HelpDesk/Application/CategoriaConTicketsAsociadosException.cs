namespace HelpDesk.Application
{
    public class CategoriaConTicketsAsociadosException : Exception
    {
        public CategoriaConTicketsAsociadosException(int id) : base($"La categoría con id {id} tiene tickets asociados y no se puede eliminar")
        {
        }
    }
}