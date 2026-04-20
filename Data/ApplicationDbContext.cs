using Microsoft.EntityFrameworkCore; 
using CalculadoraNotasAPI.Models;

namespace CalculadoraNotasAPI.Data
{
    public class ApplicationDbContext : DbContext 
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Alumno> Alumnos {get; set;} // Tabla de alumnos
        public DbSet<Nota> Notas {get; set;} // Tabla de notas        
    }
}