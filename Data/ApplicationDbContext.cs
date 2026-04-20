using Microsoft.EntityFrameworkCore; // 1. Importamos los superpoderes del traductor
using CalculadoraNotasAPI.Models;    // 2. Importamos nuestros modelos

namespace CalculadoraNotasAPI.Data
{
    // 3. Los dos puntos ":" significan "herencia". Nuestra clase hereda los poderes de DbContext
    public class ApplicationDbContext : DbContext 
    {
        // 4. El constructor. Aquí entra la configuración de la conexión (ej. la contraseña de la base de datos)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // 5. Estas propiedades representan las tablas en la base de datos
        public DbSet<Alumno> Alumnos {get; set;} // Tabla de alumnos
        public DbSet<Nota> Notas {get; set;} // Tabla de notas        
    }
}