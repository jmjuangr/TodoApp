using Microsoft.AspNetCore.Mvc;
using AporopoApi.Data;
using Models;
using System;
using System.Collections.Generic;

namespace GestorTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<TareaItem>> GetTasks()
        {
            try
            {
                var tasks = TxtProcesador.LeerTareas();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al leer las tareas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TareaItem> GetTask(int id)
        {
            try
            {
                var tasks = TxtProcesador.LeerTareas();
                TareaItem task = null;

                foreach (var t in tasks) 
                {
                    if (t.TareaId == id)
                    {
                        task = t;
                        break; 
                    }
                }

                if (task == null)
                {
                    return NotFound("Tarea no encontrada.");
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la tarea: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<TareaItem> CreateTask(TareaItem task)
        {
            try
            {
                if (task == null || string.IsNullOrWhiteSpace(task.Titulo) || string.IsNullOrWhiteSpace(task.Descripcion))
                {
                    return BadRequest("La tarea debe tener título y descripción.");
                }

                var tasks = TxtProcesador.LeerTareas();

                
                int newId = 1; 
                if (tasks.Count > 0) 
                {
                    int maxId = 0;
                    foreach (var t in tasks)
                    {
                        if (t.TareaId > maxId)
                        {
                            maxId = t.TareaId; 
                        }
                    }
                    newId = maxId + 1; 
                }

                task.TareaId = newId;
                task.Estado = false;

                tasks.Add(task);
                TxtProcesador.CrearTareas(tasks);

                return CreatedAtAction(nameof(GetTask), new { id = task.TareaId }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la tarea: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TareaItem updatedTask)
        {
            try
            {
                var tasks = TxtProcesador.LeerTareas();
                TareaItem task = null;

                foreach (var t in tasks)
                {
                    if (t.TareaId == id)
                    {
                        task = t;
                        break;
                    }
                }

                if (task == null)
                {
                    return NotFound("Tarea no encontrada.");
                }

                // Actualizar datos si se proporcionan
                if (!string.IsNullOrWhiteSpace(updatedTask.Titulo))
                {
                    task.Titulo = updatedTask.Titulo;
                }
                if (!string.IsNullOrWhiteSpace(updatedTask.Descripcion))
                {
                    task.Descripcion = updatedTask.Descripcion;
                }
                if (updatedTask.Fecha != default)
                {
                    task.Fecha = updatedTask.Fecha;
                }
                task.Estado = updatedTask.Estado;
                task.Prioridad = updatedTask.Prioridad;

                TxtProcesador.CrearTareas(tasks);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                var tasks = TxtProcesador.LeerTareas();
                TareaItem task = null;

                foreach (var t in tasks)
                {
                    if (t.TareaId == id)
                    {
                        task = t;
                        break;
                    }
                }

                if (task == null)
                {
                    return NotFound("Tarea no encontrada.");
                }

                tasks.Remove(task);
                TxtProcesador.CrearTareas(tasks);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarea: {ex.Message}");
            }
        }
    }
}
