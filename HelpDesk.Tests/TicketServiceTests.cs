using HelpDesk.Application.DTOs;
using HelpDesk.Application.Services;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Tests;

public class FakeDbContext : IAppDbContext
{
    private readonly DbContext _context;

    public FakeDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public DbSet<Ticket> Tickets => _context.Set<Ticket>();
    public DbSet<Categoria> Categorias => _context.Set<Categoria>();
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
}

public class TicketServiceTests
{
    [Fact]
    public async Task CrearTicket_CategoriaNoExiste_LanzaKeyNotFoundException()
    {
        // Arrange
        var context = new FakeDbContext();
        var service = new TicketService(context);
        var dto = new CrearTicketDto
        {
            Titulo = "Test",
            Descripcion = "Descripcion",
            Prioridad = TicketPrioridad.Alta,
            CategoriaId = 999
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.CrearTicketAsync(dto, creadorId: 1));
    }

    [Fact]
    public async Task CrearTicket_CategoriaExiste_RetornaTicketConEstadoAbierto()
    {
        // Arrange
        var context = new FakeDbContext();
        var categoria = new Categoria { Id = 1, Nombre = "Soporte", Activa = true };
        context.Categorias.Add(categoria);
        await context.SaveChangesAsync();

        var service = new TicketService(context);
        var dto = new CrearTicketDto
        {
            Titulo = "Mi ticket",
            Descripcion = "Descripcion del ticket",
            Prioridad = TicketPrioridad.Media,
            CategoriaId = 1
        };

        // Act
        var result = await service.CrearTicketAsync(dto, creadorId: 1);

        // Assert
        Assert.Equal(TicketEstado.Abierto, result.Estado);
        Assert.Equal("Mi ticket", result.Titulo);
    }
}
