namespace PokemonTCGStore.API.Models
{
    public class Carta
    {
        public int Id { get; set; }
        public string PokemonTcgId { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Rareza { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public string SetNombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public string Descripcion { get; set; } = string.Empty;
        public string Ilustrador { get; set; } = string.Empty;
    }
}