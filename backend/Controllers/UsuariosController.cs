using Microsoft.AspNetCore.Mvc;
using AporopoApi.Data;
using Models;
using System;
using System.Collections.Generic;

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
                bool usuarioExiste = false;

                // Verificar si el usuario ya existe recorriendo la lista con foreach
                foreach (var u in users)
                {
                    if (u.NombreUsuario.ToLower() == newUser.NombreUsuario.ToLower())
                    {
                        usuarioExiste = true;
                        break; // Si lo encontramos, salimos del bucle
                    }
                }

                if (usuarioExiste)
                {
                    return BadRequest("El nombre de usuario ya está en uso.");
                }

                // Generar un nuevo ID manualmente sin usar Max()
                int newId = 1;
                if (users.Count > 0) // Si hay usuarios, buscamos el ID más alto
                {
                    int maxId = 0;
                    foreach (var u in users)
                    {
                        if (u.Id > maxId)
                        {
                            maxId = u.Id; // Guardamos el ID más alto encontrado
                        }
                    }
                    newId = maxId + 1; // El nuevo ID será el más alto + 1
                }

                newUser.Id = newId;

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
                Usuario user = null;

                // Buscar el usuario manualmente sin FirstOrDefault
                foreach (var u in users)
                {
                    if (u.NombreUsuario.ToLower() == loginUser.NombreUsuario.ToLower() &&
                        u.Password == loginUser.Password)
                    {
                        user = u;
                        break; // Salimos del bucle si encontramos el usuario
                    }
                }

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
