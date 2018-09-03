using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHyperBot.Modules.Lol.Dependencies
{
    public partial class LoLLeague
    {
        [JsonProperty("queueType")] public string QueueType { get; set; }

        [JsonProperty("hotStreak")] public bool HotStreak { get; set; }

        [JsonProperty("wins")] public long Wins { get; set; }

        [JsonProperty("veteran")] public bool Veteran { get; set; }

        [JsonProperty("losses")] public long Losses { get; set; }

        [JsonProperty("playerOrTeamId")] public string PlayerOrTeamId { get; set; }

        [JsonProperty("leagueName")] public string LeagueName { get; set; }

        [JsonProperty("playerOrTeamName")] public string PlayerOrTeamName { get; set; }

        [JsonProperty("inactive")] public bool Inactive { get; set; }

        [JsonProperty("rank")] public string Rank { get; set; }

        [JsonProperty("freshBlood")] public bool FreshBlood { get; set; }

        [JsonProperty("leagueId")] public string LeagueId { get; set; }

        [JsonProperty("tier")] public string Tier { get; set; }

        [JsonProperty("leaguePoints")] public long LeaguePoints { get; set; }
    }

    public partial class LoLLeague
    {
        public static LoLLeague[] FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LoLLeague[]>(json, LoLLeagueConverter.Settings);
        }
    }

    internal static class LoLLeagueConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }
}