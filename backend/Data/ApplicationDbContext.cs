using Microsoft.EntityFrameworkCore;
using Models;

namespace TodoAppApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

       
        public DbSet<TareaItem> Tareas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
