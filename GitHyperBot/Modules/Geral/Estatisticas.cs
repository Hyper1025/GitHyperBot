using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Core;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Geral
{
    public class Estatisticas : ModuleBase<SocketCommandContext>
    {
        [Command("BotInfo")]
        [Alias("stats", "status")]
        [Summary("Mostra informações sobre o bot")]
        [CmdCategory(Categoria = CmdCategory.Geral)]
        public async Task StatisticsAsync()
        {
            var statisticsBuilder = new EmbedBuilder();
            statisticsBuilder.WithAuthor($"{Global.Client.CurrentUser.Username} statistics",
                    Global.Client.CurrentUser.GetAvatarUrl())
                .WithThumbnailUrl(Global.Client.CurrentUser.GetAvatarUrl())
                .WithColor(Color.Blue)
                .AddInlineField("Servidores", Global.Client.Guilds.Count.ToString())
                .AddInlineField("Usuários", Global.Client.Guilds.Sum(g => g.Users.Count(u => !u.IsBot)).ToString())
                .AddInlineField("Canais de texto", Global.Client.Guilds.Sum(g => g.TextChannels.Count).ToString())
                .AddInlineField("Canais de voz", Global.Client.Guilds.Sum(g => g.VoiceChannels.Count).ToString())               
                .AddInlineField("Latência", Global.Client.Latency + " ms")
                .AddInlineField("Comandos", Global.CommandService.Commands.Count().ToString());

            await ReplyAsync("", false, statisticsBuilder.Build());
        }

        [Command("ServerStatus")]
        [Alias("ServerInfo", "Server", "SS")]
        [Summary("Mostra informações sobre o servidor atual")]
        [CmdCategory(Categoria = CmdCategory.Geral)]
        public async Task GuildStatisticsAsync()
        {
            var guildstatisticsBuilder = new EmbedBuilder();
            guildstatisticsBuilder.WithAuthor($"Informações de: {Context.Guild.Name}", Context.Guild.IconUrl)
                .WithColor(Color.Blue)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .AddInlineField("Canais de texto", $"```fix\n{Context.Guild.TextChannels.Count}```")
                .AddInlineField("Canais de voz", $"```fix\n{Context.Guild.VoiceChannels.Count}```")
                .AddInlineField("Usuários", $"```fix\n{Context.Guild.MemberCount}```")
                .AddInlineField("Bots", $"```fix\n{Context.Guild.Users.Count(u => u.IsBot)}```")
                //.AddInlineField("Dono", Context.Guild.Owner.Mention)
                .AddInlineField("Cargos", $"```fix\n{Context.Guild.Roles.Count - 1}```")
                .AddInlineField("Criado em", $"```fix\n{Context.Guild.CreatedAt.Date.Day}/{Context.Guild.CreatedAt.Date.Month}/{Context.Guild.CreatedAt.Date.Year}```")
                .AddInlineField("Server ID", $"```fix\n{Context.Guild.Id}```")
                .AddInlineField("Administradores", $"```fix\n{Context.Guild.Users.Count(x => x.GuildPermissions.Has(GuildPermission.Administrator) && !x.IsBot)}```")
                .AddInlineField("Região do servidor", $"```fix\n{Context.Guild.VoiceRegionId}```");

            await ReplyAsync("", false, guildstatisticsBuilder.Build());
        }
    }
}