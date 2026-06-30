namespace HelpDesk.Application
{
    public class CategoriaNoEncontradaException : Exception
    {
        public CategoriaNoEncontradaException(int id) : base($"No se encontró la categoría con id {id}")
        {
        }
    }
}