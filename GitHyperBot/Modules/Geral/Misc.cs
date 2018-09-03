using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Admin.Dependencies;
using GitHyperBot.Modules.Help.Dependencies;
using Newtonsoft.Json;

//using GitHyperBot.Core.Databaset.User;

namespace GitHyperBot.Modules.Geral
{
    //  <summary>
    //  Essa classe contém comandos
    //  que pódem serem executados
    //  por usuários
    //  </summary>
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("Gith")]
        [Summary("Você obtem o meu repositório no GitHub")]
        [CmdCategory(Categoria = CmdCategory.Geral)]
        internal async Task CreditosTask()
        {
            await ReplyAsync("Aqui está meu [GitHub](https://github.com/hyper1025/GitHyperBot).");
        }

        //  Olá
        [Command("Olá")]
        [Alias("oi")]
        [Summary("Retorna uma mensagem de olá")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task OláTask()
        {
            await ReplyAsync($"Olá {Context.User.Mention}");
        }

        //  Invite
        [Command("Invite")]
        [Alias("Invites", "Convite", "Convites")]
        [Summary("Envia um convite do servidor para o usuário")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task PegarConvitesTask()
        {
            var convites = await Context.Guild.GetInvitesAsync();
            var ordenar = convites.OrderByDescending(x => x.Uses);
            var link = ordenar.First().Url;

            await ReplyAsync(link);
        }

        //  UpTime
        [Command("Uptime")]
        [Alias("ut")]
        [Summary("Mostra a quanto tempo o bot está ligado")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task UptimeTask()
        {
            await ReplyAsync("", false, EmbedHandler.CriarEmbed("Online!", string.Format("{0:%h} horas {0:%m} minutos {0:%s} segundos", DateTime.Now - Process.GetCurrentProcess().StartTime),EmbedMessageType.Info,false));
        }

        //  Avatar
        [Command("Avatar")]
        [Alias("Av")]
        [Summary("Pega o avatar do usuário mencionado")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task AvatarTask(IGuildUser usuário = null)
        {
            if (usuário == null) usuário = (IGuildUser) Context.User;

                await Context.Channel.SendMessageAsync("", false, EmbedHandler.CriarEmbedComImagem("Avatar", usuário.GetAvatarUrl(ImageFormat.Auto, 512),
                    $"[Link Direto]({usuário.GetAvatarUrl(ImageFormat.Auto, 1024)})"));
        }

        //  Votar
        [Command("Votar")]
        [Summary("Abro uma votação, com opções de Sim e Não.")]
        [RequireBotPermission(ChannelPermission.AddReactions)]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task NovoVotoTask([Remainder] string motivoDaVotação)
        {
            if (motivoDaVotação.Length >= 200)
            {
                await Context.Channel.SendMessageAsync("Perdão, porém seu voto não deve ter mais de 200 caracteres");
                return;
            }

            var msg = await ReplyAsync("", false,
                EmbedHandler.CriarEmbed("Vote!", motivoDaVotação, EmbedMessageType.Confused, false, Context.User));

            await msg.AddReactionAsync(new Emoji("✅"));
            await msg.AddReactionAsync(new Emoji("❌"));

            await Context.Message.DeleteAsync();

        }

        //  Neko
        [Command("Neko")]
        [Summary("Envia a imagem de uma neko")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task NekoTask()
        {
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/neko");
            }

            var jsonData = JsonConvert.DeserializeObject<dynamic>(json);
            string imageUrl = jsonData.neko;

            await ReplyAsync("", false, EmbedHandler.CriarEmbedComImagem("Gatinho", imageUrl));
        }

        [Command("Warns")]
        [Summary("Mostra a quantidade de warns para uma conta")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task WarnsUsuárioTask(SocketGuildUser usuário = null)
        {
            if (usuário == null)
            {
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Warns",$"Você tem {WarnService.WarnsTask((SocketGuildUser)Context.User)} warns.",EmbedMessageType.Info,false));
            }
            else
            {
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Warns", $"Você tem {WarnService.WarnsTask(usuário)} warns.", EmbedMessageType.Info, false));
            }
        }
    }
}