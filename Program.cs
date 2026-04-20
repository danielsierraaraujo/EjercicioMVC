using Microsoft.EntityFrameworkCore;
using CalculadoraNotasAPI.Data;
using CalculadoraNotasAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(opciones => {
    opciones.AddPolicy("PermitirReact", politica => {
        politica.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseInMemoryDatabase("UniversidadDB")
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("PermitirReact");
// DATA SEEDING: Crear datos de prueba al iniciar

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // 1. Creamos tu perfil de estudiante
    var estudianteTest = new Alumno { Name = "Daniel Cárdenas", IdBanner = "17075" };
    db.Alumnos.Add(estudianteTest);
    
    // 2. Te asignamos notas (Progreso 1: 7.0 | Progreso 2: 9.0)
    db.Notas.Add(new Nota { NotaValor = 7.0m, Progreso = 1, IdBanner = "17075", Alumno = estudianteTest });
    db.Notas.Add(new Nota { NotaValor = 9.0m, Progreso = 2, IdBanner = "17075", Alumno = estudianteTest });
    
    // 3. Guardamos los cambios
    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("PermitirReact");
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
