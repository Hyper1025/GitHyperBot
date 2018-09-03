using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHyperBot.Modules.Lol.Dependencies
{
    public class LoLVersionsData
    {
        public static string[] FromJson(string json)
        {
            return JsonConvert.DeserializeObject<string[]>(json, LoLVersionsDataConverter.Settings);
        }
    }

    //public static class LoLVersionsDataSerialize
    //{
    //    public static string ToJson(this string[] self)
    //    {
    //        return JsonConvert.SerializeObject(self, LoLVersionsDataConverter.Settings);
    //    }
    //}

    internal static class LoLVersionsDataConverter
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