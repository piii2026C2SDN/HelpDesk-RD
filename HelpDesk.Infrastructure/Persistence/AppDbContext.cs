using HelpDesk.Application.Services;
using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
            e.Property(t => t.Descripcion).IsRequired();
            e.HasOne(t => t.Categoria)
             .WithMany(c => c.Tickets)
             .HasForeignKey(t => t.CategoriaId);
        });

        modelBuilder.Entity<Categoria>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            e.HasIndex(c => c.Nombre).IsUnique();
        });
    }
}
