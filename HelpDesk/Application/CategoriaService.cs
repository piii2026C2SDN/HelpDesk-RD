using HelpDesk.Data;
using HelpDesk.Domain;
using HelpDesk.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Application
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HelpDeskDbContext _context;

        public CategoriaService(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto)
        {
            bool existe = await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existe)
            {
                throw new NombreDuplicadoException(dto.Nombre);
            }

            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Activa = true
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return MapearADto(categoria);
        }

        public async Task<List<CategoriaDto>> ListarActivasAsync()
        {
            var categorias = await _context.Categorias
                .Where(c => c.Activa)
                .ToListAsync();

            return categorias.Select(MapearADto).ToList();
        }

        public async Task<CategoriaDto> ObtenerPorIdAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                throw new CategoriaNoEncontradaException(id);
            }

            return MapearADto(categoria);
        }

        public async Task<CategoriaDto> ActualizarAsync(int id, ActualizarCategoriaDto dto)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                throw new CategoriaNoEncontradaException(id);
            }

            bool existeOtraConEseNombre = await _context.Categorias
                .AnyAsync(c => c.Id != id && c.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existeOtraConEseNombre)
            {
                throw new NombreDuplicadoException(dto.Nombre);
            }

            categoria.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();

            return MapearADto(categoria);
        }

        public async Task EliminarAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                throw new CategoriaNoEncontradaException(id);
            }

            bool tieneTickets = await _context.Tickets
                .AnyAsync(t => t.CategoriaId == id);

            if (tieneTickets)
            {
                throw new CategoriaConTicketsAsociadosException(id);
            }

            categoria.Activa = false;
            await _context.SaveChangesAsync();
        }

        private static CategoriaDto MapearADto(Categoria categoria)
        {
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Activa = categoria.Activa
            };
        }
    }
}