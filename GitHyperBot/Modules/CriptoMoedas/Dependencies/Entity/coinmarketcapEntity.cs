// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using GitHyperBot.Core.Ajuda.Dependencies;
//
//    var coinmarketcapEntity = CoinmarketcapEntity.FromJson(jsonString);

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHyperBot.Modules.CriptoMoedas.Dependencies
{
    public partial class CoinmarketcapEntity
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }

    public class Data
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("website_slug")]
        public string WebsiteSlug { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("circulating_supply")]
        public long CirculatingSupply { get; set; }

        [JsonProperty("total_supply")]
        public long TotalSupply { get; set; }

        [JsonProperty("max_supply")]
        public long MaxSupply { get; set; }

        [JsonProperty("quotes")]
        public Quotes Quotes { get; set; }

        [JsonProperty("last_updated")]
        public long LastUpdated { get; set; }
    }

    public class Quotes
    {
        [JsonProperty("USD")]
        public Usd Usd { get; set; }
    }

    public class Usd
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("volume_24h")]
        public long Volume24H { get; set; }

        [JsonProperty("market_cap")]
        public long MarketCap { get; set; }

        [JsonProperty("percent_change_1h")]
        public double PercentChange1H { get; set; }

        [JsonProperty("percent_change_24h")]
        public double PercentChange24H { get; set; }

        [JsonProperty("percent_change_7d")]
        public double PercentChange7D { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }

    public partial class CoinmarketcapEntity
    {
        public static CoinmarketcapEntity FromJson(string json) => JsonConvert.DeserializeObject<CoinmarketcapEntity>(json, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
