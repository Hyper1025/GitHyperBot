using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Modules.Gif.Dependencies;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Gif
{
    public class CmdsGif : ModuleBase<SocketCommandContext>
    {
        [Command("Gif")]
        [Summary("Responde com um gif aleatório baseado na pesquisa do usuário")]
        [CmdCategory(Categoria = CmdCategory.Diversão)]
        public async Task Gif([Remainder] string pesquisa)
        {
            await GifService.PegarGif(pesquisa, Context.Channel as SocketTextChannel);
        }
    }
}