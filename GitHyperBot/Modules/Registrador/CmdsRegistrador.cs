using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Help.Dependencies;
using GitHyperBot.Modules.Registrador.Dependencies;

namespace GitHyperBot.Modules.Registrador
{
    public class CmdsRegistrador : ModuleBase<SocketCommandContext>
    {
        [Command("Registrador")]
        [Alias("Reg")]
        [Summary("Mostra perfil de registrador")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [CmdCategory(Categoria = CmdCategory.Registrador)]
        internal async Task PerfilRegistradorTask(SocketGuildUser usuário = null)
        {
            if (usuário == null) usuário = (SocketGuildUser) Context.User;

            await RegistradorService.RegistradosTask(usuário, (SocketTextChannel) Context.Channel);
        }

        [Command("Registrar")]
        [Summary("Concluir registro de um usuário")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [CmdCategory(Categoria = CmdCategory.Registrador)]
        internal async Task RegistrarUsuárioTask(SocketGuildUser usuário)
        {
            await RegistradorService.RegistradorTask((SocketGuildUser) Context.User, usuário,
                (SocketTextChannel) Context.Channel);
        }

        [Command("Registrou")]
        [Summary("Diz para você quem registrou o usuário")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        internal async Task RegistrouTask(SocketGuildUser usuário = null)
        {
            if (usuário == null) usuário = (SocketGuildUser)Context.User;

            var userAccount = AccountsMananger.GetAccount(usuário, usuário.Guild);

            if (userAccount.MeRegistrouId == 0)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("", "Você ainda não foi registrado", EmbedMessageType.Error,
                        false, usuário));
                return;
            }

            await ReplyAsync("", false,
                EmbedHandler.CriarEmbed("", $"Registrado por <@{userAccount.MeRegistrouId}>", EmbedMessageType.Info,
                    false, usuário));
        }
    }
}