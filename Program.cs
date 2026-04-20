using Microsoft.EntityFrameworkCore;
using CalculadoraNotasAPI.Data;
using CalculadoraNotasAPI.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DEL PUERTO (Vital para Render)
var portString = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var port = int.Parse(portString);

builder.WebHost.ConfigureKestrel(opciones =>
{
    opciones.Listen(IPAddress.Any, port); 
});

// 2. AGREGAR SERVICIOS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregamos el servicio de CORS
builder.Services.AddCors();

// Base de datos en memoria
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseInMemoryDatabase("UniversidadDB")
);

var app = builder.Build();

// 3. CONFIGURACIÓN DEL PIPELINE (EL ORDEN IMPORTA AQUÍ)

// Swagger como página principal
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculadora API v1");
    c.RoutePrefix = string.Empty; 
});

// CONFIGURACIÓN GLOBAL DE CORS (Sin nombres, permiso total)
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // permite cualquier origen
    .AllowCredentials());

app.MapControllers();

// 4. DATOS INICIALES (Para que no esté vacía al arrancar)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var estudianteTest = new Alumno { Name = "Daniel Cárdenas", IdBanner = "17075" };
    db.Alumnos.Add(estudianteTest);
    db.Notas.Add(new Nota { NotaValor = 7.0m, Progreso = 1, IdBanner = "17075", Alumno = estudianteTest });
    db.Notas.Add(new Nota { NotaValor = 9.0m, Progreso = 2, IdBanner = "17075", Alumno = estudianteTest });
    db.SaveChanges();
}

app.MapGet("/ping", () => "API en línea y con CORS abierto");

app.Run();