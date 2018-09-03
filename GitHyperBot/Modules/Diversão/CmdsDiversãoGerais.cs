using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Help.Dependencies;
using Newtonsoft.Json;

namespace GitHyperBot.Modules.Diversão
{
    public class CmdsDiversãoGerais : ModuleBase<SocketCommandContext>
    {
        //  Cachorro
        [Command("Cachorro")]
        [Alias("dog")]
        [Summary("Envia a imagem de um cachorrinho pra você")]
        [CmdCategory(Categoria = CmdCategory.Diversão)]
        internal async Task CachorroTask()
        {
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString("https://random.dog/woof.json");
            }

            var jsonData = JsonConvert.DeserializeObject<dynamic>(json);
            string imageUrl = jsonData.url;

            if (imageUrl == null || imageUrl.ToLower().EndsWith("mp4"))
            {
                await CachorroTask();
            }

            await ReplyAsync("", false, EmbedHandler.CriarEmbedComImagem("Cachorro", imageUrl));
        }

        //  Siu ou não
        [Command("Sim ou Não")]
        [Alias("Sim ou Nao", "S ou N", "Yes or No")]
        [Summary("Responde com Sim ou Não")]
        [CmdCategory(Categoria = CmdCategory.Diversão)]
        internal async Task SimOuNãoTask([Remainder] string pergunta = null)
        {
            var wc = new WebClient();
            var json = wc.DownloadString("https://yesno.wtf/api/");
            var response = JsonConvert.DeserializeObject<dynamic>(json);
            var builder = new EmbedBuilder();

            string imageUrl = response.image;
            string veredito = response.answer;
            string resposta;

            switch (veredito)
            {
                case "yes":
                    builder.WithColor(Color.Green);
                    resposta = "Sim";
                    break;
                case "no":
                    builder.WithColor(Color.Red);
                    resposta = "Não";
                    break;
                default:
                    builder.WithColor(Color.Orange);
                    resposta = "Não sei";
                    break;
            }

            builder.AddField("Minha resposta é", resposta);
            builder.WithImageUrl(imageUrl);

            await ReplyAsync("", false, builder);
        }

        //  Aquela Carinha
        [Command("Aquela carinha")]
        [Alias("lenny face", "lenny")]
        [Summary("Aquela carinha...")]
        [CmdCategory(Categoria = CmdCategory.Diversão)]
        internal async Task AquelaCarinhaTask()
        {
            var carinhas = new[]
            {
                "( ͡° ͜ʖ ͡°)", "(ᴗ ͜ʖ ᴗ)", "(⟃ ͜ʖ ⟄) ", "ʕ ͡° ʖ̯ ͡°ʔ", "( ͠° ͟ʖ ͡°)", "( ͡~ ͜ʖ ͡°)", "( ͡◉ ͜ʖ ͡◉)", "( ͡° ͜V ͡°)",
                "( ͡ᵔ ͜ʖ ͡ᵔ )", "( ° ͜ʖ °)", "( ‾ ʖ̫ ‾)", "( ͡° ʖ̯ ͡°)", "( ͡° ل͜ ͡°)", "( ͡o ͜ʖ ͡o)", "( ͡☉ ͜ʖ ͡☉)", "ʕ ͡° ͜ʖ ͡°ʔ", "( ͡° ͜ʖ ͡ °)"
            };
            var r = new Random();

            await Context.Channel.SendMessageAsync(carinhas[r.Next(0, carinhas.Length)]);
        }

        //  Swag
        [Command("swag")]
        [Summary("Apenas estilo")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [CmdCategory(Categoria = CmdCategory.Diversão)]
        internal async Task SwagTask()
        {
            var msg = await ReplyAsync("( ͡° ͜ʖ ͡°)>⌐■-■");
            await Task.Delay(1500);
            await msg.ModifyAsync(x => { x.Content = "( ͡⌐■ ͜ʖ ͡-■)"; });
        }

        //  Dados
        [Command("Dados")]
        [Alias("Dado")]
        [Summary("Rola um dado")]
        [CmdCategory(Categoria = CmdCategory.Diversão)]
        internal async Task RolarDadosTask()
        {
            var r = new Random();
            var emb = new EmbedBuilder();

            emb.WithTitle("Dados");
            emb.WithDescription($"🎲 Rolado: {r.Next(1, 6)}");
            emb.WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)));
            await ReplyAsync("", false, emb);
        }
        
        //  JokeBan
        [Command("JokeBan")]
        [Alias("jban")]
        [Summary("Apenas um aviso de ban falso, somente para brincar no chat")]
        [CmdCategory(Categoria = CmdCategory.Diversão)]
        internal async Task JokeBan(SocketGuildUser usuário, [Remainder]string razão)
        {
            var m = await ReplyAsync("", false,
                EmbedHandler.CriarEmbedBan((SocketGuildUser) Context.User, usuário, razão, true));
            await Task.Delay(10000);
            await m.DeleteAsync();
        }
    }
}