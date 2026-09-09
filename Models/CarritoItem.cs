namespace PokemonTCGStore.API.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int CartaId { get; set; }
        public Carta? Carta { get; set; }

        public int Cantidad { get; set; }
    }
}