using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Data;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TareasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaItem>>> GetTasks()
        {
            try
            {
                return await _context.Tareas.ToListAsync();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al obtener las tareas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TareaItem>> GetTask(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null)
                    return NotFound("Tarea no encontrada.");

                return tarea;
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al obtener la tarea: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TareaItem>> CreateTask(TareaItem tarea)
        {
            try
            {
                if (tarea == null || string.IsNullOrWhiteSpace(tarea.Titulo) || string.IsNullOrWhiteSpace(tarea.Descripcion))
                {
                    return BadRequest("La tarea debe tener título y descripción.");
                }

                tarea.Estado = false;
                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTask), new { id = tarea.TareaId }, tarea);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al crear la tarea: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TareaItem updatedTask)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null) return NotFound("Tarea no encontrada.");

                tarea.Titulo = updatedTask.Titulo;
                tarea.Descripcion = updatedTask.Descripcion;
                tarea.Fecha = updatedTask.Fecha;
                tarea.Estado = updatedTask.Estado;
                tarea.Prioridad = updatedTask.Prioridad;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null) return NotFound("Tarea no encontrada.");

                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarea: {ex.Message}");
            }
        }
    }
}
