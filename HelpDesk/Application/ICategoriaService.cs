using HelpDesk.DTOs;

namespace HelpDesk.Application
{
    public interface ICategoriaService
    {
        Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto);
        Task<List<CategoriaDto>> ListarActivasAsync();
        Task<CategoriaDto> ObtenerPorIdAsync(int id);
        Task<CategoriaDto> ActualizarAsync(int id, ActualizarCategoriaDto dto);
        Task EliminarAsync(int id);
    }
}