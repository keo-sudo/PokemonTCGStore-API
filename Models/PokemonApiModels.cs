using System.Text.Json.Serialization;

namespace PokemonTCGStore.API.Models
{
    public class PokemonApiResponse
    {
        [JsonPropertyName("data")]
        public List<PokemonApiCard> Data { get; set; } = new();
    }

    public class PokemonApiCard
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("types")]
        public List<string>? Types { get; set; }

        [JsonPropertyName("rarity")]
        public string? Rarity { get; set; }

        [JsonPropertyName("images")]
        public PokemonApiImages? Images { get; set; }

        [JsonPropertyName("set")]
        public PokemonApiSet? Set { get; set; }

        [JsonPropertyName("tcgplayer")]
        public TcgPlayerData? Tcgplayer { get; set; }

        [JsonPropertyName("flavorText")]
        public string? FlavorText { get; set; }

        [JsonPropertyName("artist")]
        public string? Artist { get; set; }
    }

    public class PokemonApiImages
    {
        [JsonPropertyName("small")]
        public string? Small { get; set; }
    }

    public class PokemonApiSet
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class TcgPlayerData
    {
        [JsonPropertyName("prices")]
        public Dictionary<string, TcgPlayerPriceInfo>? Prices { get; set; }
    }

    public class TcgPlayerPriceInfo
    {
        [JsonPropertyName("market")]
        public decimal? Market { get; set; }
    }
}