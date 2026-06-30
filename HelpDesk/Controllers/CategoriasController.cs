using HelpDesk.Application;
using HelpDesk.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto)
        {
            try
            {
                var categoria = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = categoria.Id }, categoria);
            }
            catch (NombreDuplicadoException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var categorias = await _service.ListarActivasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var categoria = await _service.ObtenerPorIdAsync(id);
                return Ok(categoria);
            }
            catch (CategoriaNoEncontradaException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCategoriaDto dto)
        {
            try
            {
                var categoria = await _service.ActualizarAsync(id, dto);
                return Ok(categoria);
            }
            catch (CategoriaNoEncontradaException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (NombreDuplicadoException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _service.EliminarAsync(id);
                return NoContent();
            }
            catch (CategoriaNoEncontradaException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (CategoriaConTicketsAsociadosException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
        }
    }
}