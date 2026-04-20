using Microsoft.EntityFrameworkCore;
using CalculadoraNotasAPI.Data;
using CalculadoraNotasAPI.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE RED (PUERTO Y IPV4)
// ==========================================
var portString = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var port = int.Parse(portString);

builder.WebHost.ConfigureKestrel(opciones =>
{
    // Forzamos IPv4 (0.0.0.0) para que Render no dé error de conexión
    opciones.Listen(IPAddress.Any, port); 
});

// ==========================================
// 2. CONFIGURACIÓN DE SERVICIOS
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CONFIGURACIÓN DE CORS (Permiso para Vercel)
builder.Services.AddCors(opciones => {
    opciones.AddPolicy("PermitirTodo", politica => {
        politica.AllowAnyOrigin()   // Esto permite que Vercel se conecte sin problemas
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

// Base de datos en memoria
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseInMemoryDatabase("UniversidadDB")
);

var app = builder.Build();

// ==========================================
// 3. CONFIGURACIÓN DEL PIPELINE (MIDDLEWARE)
// ==========================================

// Swagger siempre visible para pruebas
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculadora API v1");
    c.RoutePrefix = string.Empty; 
});

// ACTIVAR CORS (Debe ir antes de MapControllers)
app.UseCors("PermitirTodo");

app.MapControllers();

// ==========================================
// 4. DATOS INICIALES Y RUTA DE PRUEBA
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

// Ruta para verificar que la API está arriba
app.MapGet("/ping", () => "¡API lista y con CORS abierto!");

app.Run();