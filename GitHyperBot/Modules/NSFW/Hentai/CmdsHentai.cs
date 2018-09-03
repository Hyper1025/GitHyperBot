//  Creditos: xSleepy

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Help.Dependencies;
using GitHyperBot.Modules.NSFW.Hentai.Dependencies;
using Newtonsoft.Json;

namespace GitHyperBot.Modules.NSFW.Hentai
{
    public class CmdsHentai : ModuleBase<SocketCommandContext>
    {
        [RequireNsfw]
        [Command("rule34")]
        [Alias("r34")]
        [Summary("Envia uma publicação aleatória do Rule34")]
        [CmdCategory(Categoria = CmdCategory.Nsfw)]
        public async Task Rule34Async([Remainder] string tags = null)
        {
            var taglist = new List<string>();
            if (tags != null) taglist = tags.Split(' ').ToList();
            await HentaiService.SendHentaiAsync(new HttpClient(), new Random(), HentaiService.NsfwType.Rule34,
                taglist.ToList(), Context.Channel);
        }

        [RequireNsfw]
        [Command("NsfwNeko")]
        [Alias("+18neko","18neko")]
        [Summary("Envia a imagem de uma neko")]
        [CmdCategory(Categoria = CmdCategory.Nsfw)]
        public async Task NekoTask()
        {
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/lewd/neko");
            }

            var jsonData = JsonConvert.DeserializeObject<dynamic>(json);
            string imageUrl = jsonData.neko;

            await ReplyAsync("", false, EmbedHandler.CriarEmbedComImagem("Gatinho +18", imageUrl));
        }

        [RequireNsfw]
        [Command("Cureninja")]
        [Alias("cninja", "cn")]
        [Summary("Envia uma publicação aleatória do cureninja")]
        [CmdCategory(Categoria = CmdCategory.Nsfw)]
        public async Task CureninjaAsync([Remainder] string tags = null)
        {
            var taglist = new List<string>();
            if (tags != null) taglist = tags.Split(' ').ToList();
            await HentaiService.SendHentaiAsync(new HttpClient(), new Random(), HentaiService.NsfwType.Cureninja,
                taglist.ToList(), Context.Channel);
        }

        [RequireNsfw]
        [Command("Gelbooru")]
        [Alias("gbooru", "gelb", "gb")]
        [Summary("Envia uma publicação aleatória do Gelbooru")]
        [CmdCategory(Categoria = CmdCategory.Nsfw)]
        public async Task GelbooruAsync([Remainder] string tags = null)
        {
            var taglist = new List<string>();
            if (tags != null) taglist = tags.Split(' ').ToList();
            await HentaiService.SendHentaiAsync(new HttpClient(), new Random(), HentaiService.NsfwType.Gelbooru,
                taglist.ToList(), Context.Channel);
        }

        [RequireNsfw]
        [Command("Yandere")]
        [Alias("y", "yan", "yand")]
        [Summary("Envia uma publicação aleatória do Yandere")]
        [CmdCategory(Categoria = CmdCategory.Nsfw)]
        public async Task YandereAsync([Remainder] string tags = null)
        {
            var taglist = new List<string>();
            if (tags != null) taglist = tags.Split(' ').ToList();
            await HentaiService.SendHentaiAsync(new HttpClient(), new Random(), HentaiService.NsfwType.Yandere,
                taglist.ToList(), Context.Channel);
        }
    }
}