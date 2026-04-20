using Microsoft.AspNetCore.Mvc;
using CalculadoraNotasAPI.Data;
using CalculadoraNotasAPI.Models;
using System;
using System.Linq;

namespace CalculadoraNotasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Inyección de dependencias
        public ReporteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. ENDPOINT: CONSULTAR NOTAS (GET)
        // ==========================================
        [HttpGet("calificacionesFinales/{idBanner}")]
        public IActionResult ObtenerCalificacionesFinales(string idBanner)
        {
            // Buscamos al alumno
            var alumno = _context.Alumnos.FirstOrDefault(a => a.IdBanner == idBanner);
            if (alumno == null)
            {
                return NotFound(new { error = $"No se encontró el alumno con ID Banner {idBanner}" });
            }

            // Traemos sus notas
            var notas = _context.Notas.Where(n => n.IdBanner == idBanner).ToList();
            if (!notas.Any())
            {
                return NotFound(new { error = "El alumno existe, pero aún no tiene notas registradas." });
            }

            // Lógica LINQ
            decimal notaP1 = notas.FirstOrDefault(n => n.Progreso == 1)?.NotaValor ?? 0;
            decimal notaP2 = notas.FirstOrDefault(n => n.Progreso == 2)?.NotaValor ?? 0;

            // Matemática y pesos (25% y 35%)
            decimal aporteP1 = notaP1 * 0.25m;
            decimal aporteP2 = notaP2 * 0.35m;

            decimal acumuladoActual = aporteP1 + aporteP2;

            // Fórmula de supervivencia (Meta de 6.0 y peso de 40%)
            decimal notaNecesitadaP3 = (6.0m - acumuladoActual) / 0.40m;
            notaNecesitadaP3 = Math.Round(notaNecesitadaP3, 2); // Redondeo a 2 decimales

            // Armamos el JSON de respuesta
            var reporte = new
            {
                NombreAlumno = alumno.Name,
                IdBanner = alumno.IdBanner,
                NotaFinalP1 = notaP1,
                NotaFinalP2 = notaP2,
                Acumulado = acumuladoActual,
                NotaNecesitada = notaNecesitadaP3
            };

            return Ok(reporte);
        }

        // ==========================================
        // 2. ENDPOINT: REGISTRAR ALUMNO Y NOTAS (POST)
        // ==========================================
        [HttpPost("registrar")]
        public IActionResult RegistrarAlumno([FromBody] RegistroDTO dto)
        {
            // Creamos y guardamos al alumno
            var nuevoAlumno = new Alumno { Name = dto.Name, IdBanner = dto.IdBanner };
            _context.Alumnos.Add(nuevoAlumno);
            
            // Creamos y guardamos sus notas
            var nota1 = new Nota { NotaValor = dto.NotaP1, Progreso = 1, IdBanner = dto.IdBanner, Alumno = nuevoAlumno };
            var nota2 = new Nota { NotaValor = dto.NotaP2, Progreso = 2, IdBanner = dto.IdBanner, Alumno = nuevoAlumno };
            
            _context.Notas.Add(nota1);
            _context.Notas.Add(nota2);
            
            // Guardamos todos los cambios en la base de datos
            _context.SaveChanges();

            return Ok(new { mensaje = "Alumno y notas guardados con éxito" });
        }
    }

    // ==========================================
    // CLASE AUXILIAR (DTO)
    // Molde para recibir el paquete de datos de React
    // ==========================================
    public class RegistroDTO
    {
        public string Name { get; set; } = string.Empty;
        public string IdBanner { get; set; } = string.Empty;
        public decimal NotaP1 { get; set; }
        public decimal NotaP2 { get; set; }
    }
}