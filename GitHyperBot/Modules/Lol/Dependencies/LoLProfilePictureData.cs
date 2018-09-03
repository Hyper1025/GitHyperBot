using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHyperBot.Modules.Lol.Dependencies
{
    public partial class LoLProfilePictureData
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("version")] public string Version { get; set; }

        [JsonProperty("data")] public Dictionary<string, Datum> Data { get; set; }
    }

    public class Datum
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("image")] public Image Image { get; set; }
    }

    public class Image
    {
        [JsonProperty("full")] public string Full { get; set; }

        [JsonProperty("sprite")] public Sprite Sprite { get; set; }

        [JsonProperty("group")] public Group Group { get; set; }

        [JsonProperty("x")] public long X { get; set; }

        [JsonProperty("y")] public long Y { get; set; }

        [JsonProperty("w")] public long W { get; set; }

        [JsonProperty("h")] public long H { get; set; }
    }

    public enum Group
    {
        Profileicon
    }

    public enum Sprite
    {
        Profileicon0Png
    }

    public partial class LoLProfilePictureData
    {
        public static LoLProfilePictureData FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LoLProfilePictureData>(json, LoLProfilePictureDataConverter.Settings);
        }
    }

    //public static class LoLProfilePictureDataSerialize
    //{
    //    public static string ToJson(this LoLProfilePictureData self)
    //    {
    //        return JsonConvert.SerializeObject(self, LoLProfilePictureDataConverter.Settings);
    //    }
    //}

    internal static class LoLProfilePictureDataConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new GroupConverter(),
                new SpriteConverter(),
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }

    internal class GroupConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(Group) || t == typeof(Group?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "profileicon") return Group.Profileicon;
            throw new Exception("Cannot unmarshal type Group");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Group)untypedValue;
            if (value == Group.Profileicon)
            {
                serializer.Serialize(writer, "profileicon");
                return;
            }

            throw new Exception("Cannot marshal type Group");
        }
    }

    internal class SpriteConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(Sprite) || t == typeof(Sprite?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "profileicon0.png") return Sprite.Profileicon0Png;
            throw new Exception("Cannot unmarshal type Sprite");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Sprite)untypedValue;
            if (value == Sprite.Profileicon0Png)
            {
                serializer.Serialize(writer, "profileicon0.png");
                return;
            }

            throw new Exception("Cannot marshal type Sprite");
        }
    }
}