using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHyperBot.Modules.Lol.Dependencies
{
    public partial class LoLMatchlist
    {
        [JsonProperty("matches")] public Match[] Matches { get; set; }

        [JsonProperty("startIndex")] public long StartIndex { get; set; }

        [JsonProperty("endIndex")] public long EndIndex { get; set; }

        [JsonProperty("totalGames")] public long TotalGames { get; set; }
    }

    public class Match
    {
        [JsonProperty("platformId")] public string PlatformId { get; set; }

        [JsonProperty("gameId")] public long GameId { get; set; }

        [JsonProperty("champion")] public long Champion { get; set; }

        [JsonProperty("queue")] public long Queue { get; set; }

        [JsonProperty("season")] public long Season { get; set; }

        [JsonProperty("timestamp")] public long Timestamp { get; set; }

        [JsonProperty("role")] public Role Role { get; set; }

        [JsonProperty("lane")] public Lane Lane { get; set; }
    }

    public enum Lane
    {
        Bottom,
        Jungle,
        Mid,
        None,
        Top
    }

    public enum Role
    {
        Duo,
        DuoCarry,
        DuoSupport,
        None,
        Solo
    }

    public partial class LoLMatchlist
    {
        public static LoLMatchlist FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LoLMatchlist>(json, LoLMatchlistConverter.Settings);
        }
    }

    //public static class LoLMatchlistSerialize
    //{
    //    public static string ToJson(this LoLMatchlist self)
    //    {
    //        return JsonConvert.SerializeObject(self, LoLMatchlistConverter.Settings);
    //    }
    //}

    internal static class LoLMatchlistConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new LaneConverter(),
                new RoleConverter(),
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }

    internal class LaneConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(Lane) || t == typeof(Lane?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "BOTTOM":
                    return Lane.Bottom;
                case "JUNGLE":
                    return Lane.Jungle;
                case "MID":
                    return Lane.Mid;
                case "NONE":
                    return Lane.None;
                case "TOP":
                    return Lane.Top;
            }

            throw new Exception("Cannot unmarshal type Lane");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Lane)untypedValue;
            switch (value)
            {
                case Lane.Bottom:
                    serializer.Serialize(writer, "BOTTOM");
                    return;
                case Lane.Jungle:
                    serializer.Serialize(writer, "JUNGLE");
                    return;
                case Lane.Mid:
                    serializer.Serialize(writer, "MID");
                    return;
                case Lane.None:
                    serializer.Serialize(writer, "NONE");
                    return;
                case Lane.Top:
                    serializer.Serialize(writer, "TOP");
                    return;
            }

            throw new Exception("Cannot marshal type Lane");
        }
    }

    internal class RoleConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(Role) || t == typeof(Role?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "DUO":
                    return Role.Duo;
                case "DUO_CARRY":
                    return Role.DuoCarry;
                case "DUO_SUPPORT":
                    return Role.DuoSupport;
                case "NONE":
                    return Role.None;
                case "SOLO":
                    return Role.Solo;
            }

            throw new Exception("Cannot unmarshal type Role");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Role)untypedValue;
            switch (value)
            {
                case Role.Duo:
                    serializer.Serialize(writer, "DUO");
                    return;
                case Role.DuoCarry:
                    serializer.Serialize(writer, "DUO_CARRY");
                    return;
                case Role.DuoSupport:
                    serializer.Serialize(writer, "DUO_SUPPORT");
                    return;
                case Role.None:
                    serializer.Serialize(writer, "NONE");
                    return;
                case Role.Solo:
                    serializer.Serialize(writer, "SOLO");
                    return;
            }

            throw new Exception("Cannot marshal type Role");
        }
    }
}