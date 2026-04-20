using Microsoft.EntityFrameworkCore;
using CalculadoraNotasAPI.Data;
using CalculadoraNotasAPI.Models;
using System;
using System.Net; // <--- LA LLAVE MÁGICA PARA REDES

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// EL PARCHE MAESTRO PARA RENDER (Forzar IPv4)
// ==========================================
var portString = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var port = int.Parse(portString);
builder.WebHost.ConfigureKestrel(opciones =>
{
    // Esto obliga a .NET a usar IPv4 (0.0.0.0) en lugar del conflictivo IPv6
    opciones.Listen(IPAddress.Any, port); 
});

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
    c.RoutePrefix = string.Empty; // Pone la pantalla verde al inicio
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

// Ruta de emergencia para comprobar vida
app.MapGet("/ping", () => "¡La API está viva, escuchando en IPv4 y conectada a Render!");

app.Run(); // Inicia el servidor con la red que configuramos arriba