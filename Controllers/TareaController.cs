
using Microsoft.AspNetCore.Mvc;
using ProyectoGestionTareas.Domain.Model;
using ProyectoGestionTareas.Domain.Services.Interfaces;

namespace ProyectoGestionTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {

        private readonly ITareaService _tareaService;
        private readonly ILogger<TareaController> _logger;

        public TareaController(ITareaService tareaService, ILogger<TareaController> logger)
        {
            _tareaService = tareaService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddTarea(Tarea tarea)
        {
            try
            {
                if (String.IsNullOrEmpty(tarea.Titulo) || tarea.FechaCreacion == null || tarea.FechaVencimiento == null)
                {
                    return BadRequest("You must enter all required fields");
                }

                var result = await _tareaService.AddTareaAsync(tarea);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTarea(int id, [FromBody] Tarea tarea)
        {
            try
            {
                if (id != tarea.Id)
                {
                    return BadRequest("Tarea ID mismatch");
                }

                if (String.IsNullOrEmpty(tarea.Titulo) || tarea.FechaCreacion == null || tarea.FechaVencimiento == null)
                {
                    return BadRequest("You must enter all required fields");
                }

                var existingTarea = await _tareaService.GetTareaByIdAsync(id);
                if (existingTarea == null)
                {
                    return NotFound("Tarea not found");
                }

                var result = await _tareaService.UpdateTareaAsync(tarea);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            try
            {
                var tarea = await _tareaService.GetTareaByIdAsync(id);
                if (tarea == null)
                {
                    return NotFound("Tarea not found");
                }
                await _tareaService.DeleteTareaAsync(tarea);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            try
            {
                var budget = await _tareaService.GetTareaByIdAsync(id);
                if (budget == null)
                {
                    return NotFound("Tarea not found");
                }
                return Ok(budget);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAllTarea")]
        public async Task<IActionResult> GetAllTarea()
        {
            try
            {
                var budgets = await _tareaService.GetAllTareasAsync();
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