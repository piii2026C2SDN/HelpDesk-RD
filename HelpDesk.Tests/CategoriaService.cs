using HelpDesk.Application;
using HelpDesk.Data;
using HelpDesk.Domain;
using HelpDesk.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using Xunit;

namespace HelpDesk.Tests
{
    public class CategoriaServiceTests
    {
        private static HelpDeskDbContext CrearContextoEnMemoria()
        {
            var opciones = new DbContextOptionsBuilder<HelpDeskDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new HelpDeskDbContext(opciones);
        }

        [Fact]
        public async Task Eliminar_DeberiaLanzarExcepcion_SiCategoriaTieneTicketsAsociados()
        {
            // Arrange: preparamos los datos antes de la prueba
            var context = CrearContextoEnMemoria();

            var categoria = new Categoria { Nombre = "Hardware", Activa = true };
            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();

            context.Tickets.Add(new Ticket { CategoriaId = categoria.Id });
            await context.SaveChangesAsync();

            var service = new CategoriaService(context);

            // Act + Assert: ejecutamos la acción y verificamos que lanza la excepción esperada
            await Assert.ThrowsAsync<CategoriaConTicketsAsociadosException>(
                () => service.EliminarAsync(categoria.Id));
        }

        [Fact]
        public async Task Crear_DeberiaLanzarExcepcion_SiNombreYaExiste()
        {
            // Arrange
            var context = CrearContextoEnMemoria();

            context.Categorias.Add(new Categoria { Nombre = "Software", Activa = true });
            await context.SaveChangesAsync();

            var service = new CategoriaService(context);
            var dto = new CrearCategoriaDto { Nombre = "Software" };

            // Act + Assert
            await Assert.ThrowsAsync<NombreDuplicadoException>(
                () => service.CrearAsync(dto));
        }
    }
}