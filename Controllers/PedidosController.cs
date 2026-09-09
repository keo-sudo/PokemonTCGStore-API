using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonTCGStore.API.Data;
using PokemonTCGStore.API.Models;

namespace PokemonTCGStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        // ... el resto del código se queda igual
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos/usuario/5
        // Solo trae los pedidos DE ESE usuario específico
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosPorUsuario(int usuarioId)
        {
            return await _context.Pedidos
                .Where(p => p.UsuarioId == usuarioId)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Carta)
                .ToListAsync();
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Carta)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        // POST: api/Pedidos
        // Crea un pedido completo con sus detalles de una sola vez
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            pedido.Fecha = DateTime.UtcNow;
            pedido.Estado = "Pendiente";

            // Calcula el total sumando cantidad * precio de cada detalle
            pedido.Total = pedido.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }

        // PUT: api/Pedidos/5/estado
        // Endpoint específico solo para cambiar el estado (ej: a "Enviado")
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] string nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            pedido.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}