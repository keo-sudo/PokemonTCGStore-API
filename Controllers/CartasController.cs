using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonTCGStore.API.Data;
using PokemonTCGStore.API.Models;
using System.Text.Json;

namespace PokemonTCGStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly TraduccionService _traduccionService;

        public CartasController(AppDbContext context, IHttpClientFactory httpClientFactory, TraduccionService traduccionService)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _traduccionService = traduccionService;
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

        // POST: api/Cartas/importar?setId=base1
        [Authorize]
        [HttpPost("importar")]
        public async Task<ActionResult> ImportarCartas(string setId)
        {
            try
            {
                var url = $"https://api.pokemontcg.io/v2/cards?q=set.id:{setId}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return StatusCode(500, $"Status: {response.StatusCode}, Detalle: {errorBody}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<PokemonApiResponse>(json);

                if (resultado == null || resultado.Data.Count == 0)
                {
                    return NotFound("No se encontraron cartas con ese set.");
                }

                int nuevasCartas = 0;

                foreach (var cartaApi in resultado.Data)
                {
                    bool yaExiste = await _context.Cartas.AnyAsync(c => c.PokemonTcgId == cartaApi.Id);
                    if (yaExiste) continue;

                    decimal precioReal = 5.00m; // precio de respaldo si la API no tiene dato

                    if (cartaApi.Tcgplayer?.Prices != null && cartaApi.Tcgplayer.Prices.Count > 0)
                    {
                        var primeraVariante = cartaApi.Tcgplayer.Prices.Values.FirstOrDefault();
                        if (primeraVariante?.Market != null)
                        {
                            precioReal = primeraVariante.Market.Value;
                        }
                    }

                    string descripcionEspanol = await _traduccionService.TraducirAlEspanol(cartaApi.FlavorText ?? "");

                    var carta = new Carta
                    {
                        PokemonTcgId = cartaApi.Id,
                        Nombre = cartaApi.Name,
                        Tipo = cartaApi.Types != null && cartaApi.Types.Count > 0 ? cartaApi.Types[0] : "Desconocido",
                        Rareza = cartaApi.Rarity ?? "Desconocida",
                        ImagenUrl = cartaApi.Images?.Small ?? "",
                        SetNombre = cartaApi.Set?.Name ?? "Desconocido",
                        Precio = precioReal,
                        Stock = 10,
                        Descripcion = descripcionEspanol,
                        Ilustrador = cartaApi.Artist ?? "Desconocido"
                    };

                    _context.Cartas.Add(carta);
                    nuevasCartas++;
                }

                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Se importaron {nuevasCartas} cartas nuevas.", totalEncontradas = resultado.Data.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"EXCEPCIÓN: {ex.GetType().Name} - {ex.Message}");
            }
        }
    }
}