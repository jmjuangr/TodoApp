using Microsoft.AspNetCore.Mvc;
using AporopoApi.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetUsers()
        {
            try
            {
                var users = TxtProcesador.LeerUsuarios();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al leer los usuarios: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public ActionResult<Usuario> RegisterUser([FromBody] Usuario newUser)
        {
            try
            {
                if (newUser == null || string.IsNullOrWhiteSpace(newUser.NombreUsuario) || string.IsNullOrWhiteSpace(newUser.Password))
                {
                    return BadRequest("El nombre de usuario y la contraseña son obligatorios.");
                }

                var users = TxtProcesador.LeerUsuarios();
                
                // Verificar si el usuario ya existe
                if (users.Any(u => u.NombreUsuario.ToLower() == newUser.NombreUsuario.ToLower()))
                {
                    return BadRequest("El nombre de usuario ya está en uso.");
                }

                // Generar ID automáticamente
                newUser.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

                users.Add(newUser);
                TxtProcesador.CrearUsuarios(users);

                return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar el usuario: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public ActionResult<Usuario> LoginUser([FromBody] Usuario loginUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUser.NombreUsuario) || string.IsNullOrWhiteSpace(loginUser.Password))
                {
                    return BadRequest("Usuario y contraseña son obligatorios.");
                }

                var users = TxtProcesador.LeerUsuarios();
                var user = users.FirstOrDefault(u => 
                    u.NombreUsuario.ToLower() == loginUser.NombreUsuario.ToLower() &&
                    u.Password == loginUser.Password);

                if (user == null)
                {
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al iniciar sesión: {ex.Message}");
            }
        }
    }
}
