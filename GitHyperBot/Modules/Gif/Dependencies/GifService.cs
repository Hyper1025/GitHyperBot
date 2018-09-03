using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.WebSocket;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Handlers;
using Newtonsoft.Json;

namespace GitHyperBot.Modules.Gif.Dependencies
{
    public class GifService
    {
        internal static async Task PegarGif(string pesquisa, SocketTextChannel canal)
        {
            var questão = WebUtility.UrlEncode(pesquisa);

            using (var http = new HttpClient())
            {
                var resposta = await http
                    .GetStringAsync(
                        $"http://api.giphy.com/v1/gifs/search?api_key={Config.Bot.GiphyApiKey}&lang=pt&q={Uri.EscapeUriString(questão)}")
                    .ConfigureAwait(false);

                var data = JsonConvert.DeserializeObject<GiphyData>(resposta);
                var r = new Random();

                if (data.Data.Count == 0)
                {
                    await canal.SendMessageAsync("",false,EmbedHandler.CriarEmbed("Não encontrei",$"Não encontrei nenhum gif para {pesquisa}",EmbedMessageType.Info,false));
                    return;
                }

                var randData = data.Data[r.Next(data.Data.Count)];
                var indexOf = randData.Images.Original.Url.IndexOf('?');

                var url = (indexOf == -1
                    ? randData.Images.Original.Url
                    : randData.Images.Original.Url.Remove(indexOf));

                await canal.SendMessageAsync("",false,EmbedHandler.CriarEmbedComImagem("GIF", url, $"[Link Direto]({url})", false, "Fonte: Giphy", "https://cdn.iconscout.com/public/images/icon/free/png-256/giphy-3149034d1122d7c1-256x256.png"));
            }
        }
    }
}