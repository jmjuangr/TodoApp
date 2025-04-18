namespace Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string NombreUsuario { get; set; } = "";
        public string Password { get; set; } = "";
    }