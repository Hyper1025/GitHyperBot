using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Modules.Divulgador.Dependencies;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Divulgador
{
    public class CmdsDivulgador : ModuleBase<SocketCommandContext>
    {
        [Command("Divulgador")]
        [Alias("Div")]
        [Summary("Mostra perfil de divulgador")]
        [CmdCategory(Categoria = CmdCategory.Divulgador)]
        internal async Task PerfilDivulgadorTask(SocketGuildUser usuário = null)
        {
            if (usuário == null)
            {
                await DivulgadorServices.DivulgadorTask((SocketGuildUser)Context.User, (SocketTextChannel)Context.Channel);
            }
            else
            {
                await DivulgadorServices.DivulgadorTask(usuário, (SocketTextChannel)Context.Channel);
            }
        }

        [Command("MeusConvites")]
        [Alias("Convites")]
        [Summary("Mostra perfil de divulgador")]
        [CmdCategory(Categoria = CmdCategory.Divulgador)]
        internal async Task MeusConvitesTask(SocketGuildUser usuário = null)
        {
            if (usuário == null)
            {
                await DivulgadorServices.Convites((SocketGuildUser)Context.User, 
                    (SocketTextChannel)Context.Channel);
            }
            else
            {
                await DivulgadorServices.Convites(usuário, (SocketTextChannel)Context.Channel);
            }
        }

        [Command("TopDivulgador")]
        [Alias("TopDiv")]
        [Summary("Retorna um top 5 dos divulgadores")]
        internal async Task TopDivulgadoresTask(SocketGuildUser usuário = null)
        {
            if (usuário == null)
            {
                await DivulgadorServices.TopInviters((SocketGuildUser) Context.User,
                    (SocketTextChannel) Context.Channel);
            }
            else
            {
                await DivulgadorServices.TopInviters(usuário, (SocketTextChannel) Context.Channel);
            }
        }
    }
}