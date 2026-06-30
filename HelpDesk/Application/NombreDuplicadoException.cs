namespace HelpDesk.Application
{
    public class NombreDuplicadoException : Exception
    {
        public NombreDuplicadoException(string nombre) : base($"Ya existe una categoría con el nombre '{nombre}'")
        {
        }
    }
}