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
    public class CarritoItemsController : ControllerBase
    {
        // ... el resto del código se queda igual
        private readonly AppDbContext _context;

        public CarritoItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CarritoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarritoItem>>> GetCarritoItems()
        {
            return await _context.CarritoItems
                .Include(c => c.Carta)
                .ToListAsync();
        }

        // GET: api/CarritoItems/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<CarritoItem>>> GetCarritoPorUsuario(int usuarioId)
        {
            return await _context.CarritoItems
                .Include(c => c.Carta)
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // POST: api/CarritoItems
        [HttpPost]
        public async Task<ActionResult<CarritoItem>> PostCarritoItem(CarritoItem item)
        {
            _context.CarritoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarritoItems), new { id = item.Id }, item);
        }

        // PUT: api/CarritoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarritoItem(int id, CarritoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/CarritoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarritoItem(int id)
        {
            var item = await _context.CarritoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.CarritoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}