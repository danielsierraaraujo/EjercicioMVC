using Microsoft.EntityFrameworkCore;
using CalculadoraNotasAPI.Data;
using CalculadoraNotasAPI.Models;
using System; // Necesario para leer variables de entorno

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configurar CORS (Pase VIP para React)
builder.Services.AddCors(opciones => {
    opciones.AddPolicy("PermitirReact", politica => {
        politica.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// 3. Base de Datos
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseInMemoryDatabase("UniversidadDB")
);

var app = builder.Build();

// ==========================================
// CONFIGURACIÓN DE SWAGGER
// ==========================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculadora API v1");
    c.RoutePrefix = string.Empty; // Swagger en la página de inicio
});

app.UseCors("PermitirReact");
app.MapControllers();

// ==========================================
// DATA SEEDING (Datos de prueba)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var estudianteTest = new Alumno { Name = "Daniel Cárdenas", IdBanner = "17075" };
    db.Alumnos.Add(estudianteTest);
    db.Notas.Add(new Nota { NotaValor = 7.0m, Progreso = 1, IdBanner = "17075", Alumno = estudianteTest });
    db.Notas.Add(new Nota { NotaValor = 9.0m, Progreso = 2, IdBanner = "17075", Alumno = estudianteTest });
    db.SaveChanges();
}

// ==========================================
// RUTA DE EMERGENCIA Y PUERTOS DE RENDER
// ==========================================

// Ruta infalible para probar conexión
app.MapGet("/ping", () => "¡La API está viva, escuchando en la nube y conectada!");

// Leer el puerto dinámico que Render nos exige usar
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Encender la API forzando la puerta correcta en todas las redes (0.0.0.0)
app.Run($"http://0.0.0.0:{port}");