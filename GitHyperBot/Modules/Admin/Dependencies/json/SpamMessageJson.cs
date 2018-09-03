// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using GitHyperBot.Modules.Admin.Dependencies.json;
//
//    var spamMessage = SpamMessage.FromJson(jsonString);

namespace GitHyperBot.Modules.Admin.Dependencies.json
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class SpamMessage
    {
        [JsonProperty("Mensagem")]
        public string Mensagem { get; set; }

        [JsonProperty("Titulo")]
        public string Titulo { get; set; }

        [JsonProperty("Descrição")]
        public string Descrição { get; set; }

        [JsonProperty("ImagemUrl")]
        public string ImagemUrl { get; set; }
    }

    public partial class SpamMessage
    {
        public static SpamMessage FromJson(string json) => JsonConvert.DeserializeObject<SpamMessage>(json, Converter.Settings);
    }

    //public static class Serialize
    //{
    //    public static string ToJson(this SpamMessage self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //}

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
