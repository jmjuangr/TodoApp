using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Data;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsers()
        {
            try
            {
                return await _context.Usuarios.ToListAsync();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al obtener los usuarios: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> RegisterUser([FromBody] Usuario newUser)
        {
            try
            {
                if (newUser == null || string.IsNullOrWhiteSpace(newUser.NombreUsuario) || string.IsNullOrWhiteSpace(newUser.Password))
                    return BadRequest("Nombre de usuario y contraseña son obligatorios.");

                bool existe = await _context.Usuarios.AnyAsync(u =>
                    u.NombreUsuario.ToLower() == newUser.NombreUsuario.ToLower());

                if (existe)
                    return BadRequest("El nombre de usuario ya está en uso.");

                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al registrar el usuario: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> LoginUser([FromBody] Usuario loginUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUser.NombreUsuario) || string.IsNullOrWhiteSpace(loginUser.Password))
                    return BadRequest("Usuario y contraseña son obligatorios.");

                var user = await _context.Usuarios.FirstOrDefaultAsync(u =>
                    u.NombreUsuario.ToLower() == loginUser.NombreUsuario.ToLower()
                    && u.Password == loginUser.Password);

                if (user == null)
                    return Unauthorized("Usuario o contraseña incorrectos.");

                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al iniciar sesión: {ex.Message}");
            }
        }
    }
}
