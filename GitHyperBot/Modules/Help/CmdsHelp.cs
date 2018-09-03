using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Help
{
    public class CmdsHelp : ModuleBase<SocketCommandContext>
    {
        [Command("Help")]
        [Alias("h","ajuda")]
        [Summary("Mostra lista de commandos")]
        [CmdCategory(Categoria = CmdCategory.Misc)]
        public async Task HelpTask([Remainder] string comando = null)
        {
            if (comando ==  null)
            {
                await HelpService.HelpListTask((SocketTextChannel) Context.Channel, (SocketGuildUser)Context.User);
            }
            else
            {
                await HelpService.HelpCommandTask(Context, comando);
            }
        }
    }
}