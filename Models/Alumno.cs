using System.Text.Json.Serialization;

namespace CalculadoraNotasAPI.Models
{
    public class Alumno
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IdBanner { get; set; } = string.Empty; 

        [JsonIgnore] 
        public List<Nota> Notas { get; set; } = new List<Nota>();
    }
}