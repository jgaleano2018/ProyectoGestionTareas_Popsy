using Microsoft.AspNetCore.Mvc;
using ProyectoGestionTareas.Domain.Model;
using ProyectoGestionTareas.Domain.Services;
using ProyectoGestionTareas.Domain.Services.Interfaces;

namespace ProyectoGestionTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<TareaController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<TareaController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUsuario(Usuario usuario)
        {
            try
            {
                if (String.IsNullOrEmpty(usuario.Nombre) || String.IsNullOrEmpty(usuario.Email))
                {
                    return BadRequest("You must enter all required fields");
                }

                var result = await _usuarioService.AddUsuarioAsync(usuario);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (id != usuario.Id)
                {
                    return BadRequest("Usuario ID mismatch");
                }

                if (String.IsNullOrEmpty(usuario.Nombre) || String.IsNullOrEmpty(usuario.Email))
                {
                    return BadRequest("You must enter all required fields");
                }

                var existingTarea = await _usuarioService.GetUsuarioByIdAsync(id);
                if (existingTarea == null)
                {
                    return NotFound("Usuario not found");
                }

                var result = await _usuarioService.UpdateUsuarioAsync(usuario);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var tarea = await _usuarioService.GetUsuarioByIdAsync(id);
                if (tarea == null)
                {
                    return NotFound("Usuario not found");
                }
                await _usuarioService.DeleteUsuarioAsync(tarea);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            try
            {
                var budget = await _usuarioService.GetUsuarioByIdAsync(id);
                if (budget == null)
                {
                    return NotFound("Usuario not found");
                }
                return Ok(budget);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAllUsuario")]
        public async Task<IActionResult> GetAllUsuario()
        {
            try
            {
                var budgets = await _usuarioService.GetAllUsuariosAsync();
                return Ok(budgets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

}