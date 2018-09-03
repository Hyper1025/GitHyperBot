using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Services;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Geral
{
    public class Perfil: ModuleBase<SocketCommandContext>
    {
        [Command("Perfil")]
        [Alias("Profile")]
        [Summary("Mostra o perfil do usuário")]
        [CmdCategory(Categoria = CmdCategory.Geral)]
        internal async Task PerfilTask(SocketGuildUser usuário = null)
        {
            if (usuário == null) usuário = (SocketGuildUser)Context.User;
            
            var userAccount = AccountsMananger.GetAccount(usuário, usuário.Guild);
            var guildAccount = GuildsMannanger.GetGuild(Context.Guild);
            var r = new Random();
            var eb = new EmbedBuilder();

            eb.WithAuthor(usuário.Username,
                    usuário.GetAvatarUrl() ?? $"https://cdn.discordapp.com/embed/avatars/{r.Next(0, 4)}.png")
                .WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)))
                .WithThumbnailUrl(usuário.GetAvatarUrl() ??
                                  $"https://cdn.discordapp.com/embed/avatars/{usuário.DiscriminatorValue % 5}.png")
                .AddInlineField("Identificação", $"```md\n[id]({usuário.Id})```")
                .AddInlineField("Nivelamento",
                    $"```fix\nLevel {ExpService.CalcularNivel((int) userAccount.Xp)}\nXP: {userAccount.Xp}```")
                .AddInlineField("Golds", $"```fix\nCoins: {userAccount.Gold}```")
                .AddInlineField("Advertências", $"```fix\nWarns {userAccount.NumberOfWarning}/{guildAccount.MaxWarning}```")
                .AddInlineField("Criado em",
                    $"```fix\n{usuário.CreatedAt.Day}/{usuário.CreatedAt.Month}/{usuário.CreatedAt.Year}```");

            if (userAccount.MeRegistrouId != 0)
            {
                eb.AddInlineField("Registrado por", $"<@{userAccount.MeRegistrouId}>");
            } 
            else
            {
                eb.AddInlineField("Registrado por:", "```diff\n- Ninguém```");
            }

            //  Verifica a disponibilidade do daily
            var diferença = DateTime.UtcNow - userAccount.LastDaily.AddDays(1);
            eb.AddInlineField("Daily", diferença.TotalHours < 0 ? "```prolog\nCooldown```" : "```css\nPronto\n```");   //  True : False
            //  Envia
            await ReplyAsync("", false, eb.Build());
        }
    }
}