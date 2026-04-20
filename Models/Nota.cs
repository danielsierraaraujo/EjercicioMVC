using System.Text.Json.Serialization;

namespace CalculadoraNotasAPI.Models
{
    public class Nota
    {
        public int NotaId { get; set; }
        public decimal NotaValor { get; set; } 
        public int Progreso { get; set; } 
        public string IdBanner { get; set; } = string.Empty; 

        [JsonIgnore]
        public Alumno? Alumno { get; set; }
    }
}