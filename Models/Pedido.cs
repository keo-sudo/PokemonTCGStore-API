namespace PokemonTCGStore.API.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public List<PedidoDetalle> Detalles { get; set; } = new();
    }
}