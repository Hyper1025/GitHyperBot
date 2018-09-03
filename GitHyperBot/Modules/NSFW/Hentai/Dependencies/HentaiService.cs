//  Fonte: https://github.com/xSleepy/SleepBot/blob/4b71b805b96b268462759776f29d94936c4a1aa4/Services/NSFW/HentaiService.cs
//  Só mudei algumas coisnhas, o resto é o código base que tá na fonte

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using GitHyperBot.Core.Handlers;

#endregion

namespace GitHyperBot.Modules.NSFW.Hentai.Dependencies
{
    public class HentaiService
    {
        public enum NsfwType
        {
            Rule34,
            Yandere,
            Gelbooru,
            Konachan,
            Cureninja
        }

        public static async Task SendHentaiAsync(HttpClient httpClient, Random random, NsfwType nsfwType,
            List<string> tags, IMessageChannel channel)
        {
            string url = null;
            string result = null;
            MatchCollection matches;
            tags = tags == new List<string>() ? new[] { "boobs", "tits", "ass", "sexy", "neko" }.ToList() : tags;
            switch (nsfwType)
            {
                case NsfwType.Gelbooru:
                    url =
                        $"http://gelbooru.com/index.php?page=dapi&s=post&q=index&limit=200&tags={string.Join("+", tags.Select(x => x.Replace(" ", "_")))}";
                    break;
                case NsfwType.Rule34:
                    url =
                        $"http://rule34.xxx/index.php?page=dapi&s=post&q=index&limit=200&tags={string.Join("+", tags.Select(x => x.Replace(" ", "_")))}";
                    break;
                case NsfwType.Cureninja:
                    url =
                        $"https://cure.ninja/booru/api/json?f=a&o=r&s=1&q={string.Join("+", tags.Select(x => x.Replace(" ", "_")))}";
                    break;
                case NsfwType.Konachan:
                    url =
                        $"http://konachan.com/post?page={random.Next(0, 5)}&tags={string.Join("+", tags.Select(x => x.Replace(" ", "_")))}";
                    break;
                case NsfwType.Yandere:
                    url =
                        $"https://yande.re/post.xml?limit=25&page={random.Next(0, 15)}&tags={string.Join("+", tags.Select(x => x.Replace(" ", "_")))}";
                    break;
            }

            var get = await httpClient.GetStringAsync(url).ConfigureAwait(false);
            switch (nsfwType)
            {
                case NsfwType.Yandere:
                case NsfwType.Gelbooru:
                case NsfwType.Rule34:
                    matches = Regex.Matches(get, "file_url=\"(.*?)\" ");
                    break;
                case NsfwType.Cureninja:
                    matches = Regex.Matches(get, "\"url\":\"(.*?)\"");
                    break;
                case NsfwType.Konachan:
                    matches = Regex.Matches(get, "<a class=\"directlink smallimg\" href=\"(.*?)\"");
                    break;
                default:
                    matches = Regex.Matches(get, "\"url\":\"(.*?)\"");
                    break;
            }

            switch (nsfwType)
            {
                case NsfwType.Konachan:
                case NsfwType.Gelbooru:
                case NsfwType.Yandere:
                case NsfwType.Rule34:
                    result = $"{matches[random.Next(0, matches.Count)].Groups[1].Value}";
                    break;
                case NsfwType.Cureninja:
                    result = matches[random.Next(0, matches.Count)].Groups[1].Value.Replace("\\/", "/");
                    break;
            }

            result = result != null && result.EndsWith("/") ? result.Substring(0, result.Length - 1) : result;
            await channel.SendMessageAsync("", false,
                EmbedHandler.CriarEmbedComImagem("Rule34", result, $"[Link Direto]({result})", false, $"Fonte: {nsfwType.ToString()}"));
        }
    }
}