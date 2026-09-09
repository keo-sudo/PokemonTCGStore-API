using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonTCGStore.API.Data;
using PokemonTCGStore.API.Models;

namespace PokemonTCGStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cartas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carta>>> GetCartas()
        {
            return await _context.Cartas.ToListAsync();
        }

        // GET: api/Cartas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carta>> GetCarta(int id)
        {
            var carta = await _context.Cartas.FindAsync(id);

            if (carta == null)
            {
                return NotFound();
            }

            return carta;
        }

        // POST: api/Cartas
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Carta>> PostCarta(Carta carta)
        {
            _context.Cartas.Add(carta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarta), new { id = carta.Id }, carta);
        }

        // PUT: api/Cartas/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarta(int id, Carta carta)
        {
            if (id != carta.Id)
            {
                return BadRequest();
            }

            _context.Entry(carta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Cartas/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarta(int id)
        {
            var carta = await _context.Cartas.FindAsync(id);
            if (carta == null)
            {
                return NotFound();
            }

            _context.Cartas.Remove(carta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}