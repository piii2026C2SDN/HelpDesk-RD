using HelpDesk.Application.DTOs;
using HelpDesk.Application.Interfaces;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Application.Services;

public interface IAppDbContext
{
    DbSet<Ticket> Tickets { get; }
    DbSet<Categoria> Categorias { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class TicketService : ITicketService
{
    private readonly IAppDbContext _context;

    public TicketService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<TicketResponseDto> CrearTicketAsync(CrearTicketDto dto, int creadorId)
    {
        var categoria = await _context.Categorias.FindAsync(dto.CategoriaId)
            ?? throw new KeyNotFoundException("Categoría no encontrada");

        var ticket = new Ticket
        {
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Prioridad = dto.Prioridad,
            CategoriaId = dto.CategoriaId,
            CreadorId = creadorId,
            Estado = TicketEstado.Abierto,
            FechaCreacion = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return MapToDto(ticket, categoria.Nombre);
    }

    public async Task<IEnumerable<TicketResponseDto>> ListarTicketsAsync()
    {
        return await _context.Tickets
            .Include(t => t.Categoria)
            .Select(t => new TicketResponseDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Prioridad = t.Prioridad,
                Estado = t.Estado,
                Categoria = t.Categoria.Nombre,
                FechaCreacion = t.FechaCreacion
            })
            .ToListAsync();
    }

    private static TicketResponseDto MapToDto(Ticket ticket, string categoriaNombre) => new()
    {
        Id = ticket.Id,
        Titulo = ticket.Titulo,
        Prioridad = ticket.Prioridad,
        Estado = ticket.Estado,
        Categoria = categoriaNombre,
        FechaCreacion = ticket.FechaCreacion
    };
}
