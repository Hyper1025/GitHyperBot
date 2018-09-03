using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHyperBot.Modules.Lol.Dependencies
{
    public partial class LoLProfile
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("accountId")] public long AccountId { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("profileIconId")] public long ProfileIconId { get; set; }

        [JsonProperty("revisionDate")] public long RevisionDate { get; set; }

        [JsonProperty("summonerLevel")] public long SummonerLevel { get; set; }
    }

    public partial class LoLProfile
    {
        public static LoLProfile FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LoLProfile>(json, LoLProfileConverter.Settings);
        }
    }

    //public static class LoLProfileSerialize
    //{
    //    public static string ToJson(this LoLProfile self)
    //    {
    //        return JsonConvert.SerializeObject(self, LoLProfileConverter.Settings);
    //    }
    //}

    internal static class LoLProfileConverter
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