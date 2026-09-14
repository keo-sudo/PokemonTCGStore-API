using System.Text.Json;

namespace PokemonTCGStore.API.Data
{
    public class TraduccionService
    {
        private readonly HttpClient _httpClient;

        public TraduccionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> TraducirAlEspanol(string textoIngles)
        {
            if (string.IsNullOrWhiteSpace(textoIngles))
                return string.Empty;

            try
            {
                var textoCodificado = Uri.EscapeDataString(textoIngles);
                var url = $"https://api.mymemory.translated.net/get?q={textoCodificado}&langpair=en|es";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return textoIngles; // si falla, devuelve el original

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var traduccion = doc.RootElement
                    .GetProperty("responseData")
                    .GetProperty("translatedText")
                    .GetString();

                return traduccion ?? textoIngles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error traduciendo: {ex.Message}");
            }
        }
    }
}