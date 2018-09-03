/*Thanks to xSleepy for sending me the code, referring to the league*/

using System.Threading.Tasks;
using Discord.Commands;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Help.Dependencies;
using GitHyperBot.Modules.Lol.Dependencies;

namespace GitHyperBot.Modules.Lol
{
    public class CmdsLoL : ModuleBase<SocketCommandContext>
    {
        [Command("lolprofile")]
        [Alias("lol", "league", "leaguestats","lolstatus")]
        [Summary("Mostra o seu perfil no lol")]
        [CmdCategory(Categoria = CmdCategory.Ferramentas)]
        public async Task EncodeAsync(string região, [Remainder] string nomeDeInvocador)
        {
            if (Config.Bot.LoLApiKey == null)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Meee...", "Poxa, eu não tenho uma key da api da riot, infelizmente...",
                        EmbedMessageType.AccessDenied, false));
                return;
            }
            await LoLService.SendLoLStatistics(região, nomeDeInvocador, Context.Guild.GetTextChannel(Context.Channel.Id));
        }
    }
}