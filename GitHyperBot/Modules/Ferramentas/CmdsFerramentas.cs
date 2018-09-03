using System.Threading.Tasks;
using Discord.Commands;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Ferramentas
{
    public class CmdsFerramentas : ModuleBase<SocketCommandContext>
    {
        [Command("BloquearSpam")]
        [Alias("DesativarSpam","BSpam")]
        [Summary("Desativa o envio de spam no privado.\n" +
                 "Isso não é referente a notificações, apenas spam relacionado a divulgação")]
        [CmdCategory(Categoria = CmdCategory.Ferramentas)]
        internal async Task BloquarSpamTask()
        {
            var userAccount = AccountsMananger.GetAccount(Context.User, Context.Guild);

            if (userAccount.ReceberSpam)
            {
                userAccount.ReceberSpam = false;
                AccountsMananger.SaveAccounts();
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Certinho",
                    "A partir de agora, você não será mais informado de eventos e novidades no nosso servidor.\n" +
                    $"Você pode ativar o spam novamente digitando {Config.Bot.CmdPrefix}AtivarSpam",
                    EmbedMessageType.Config, false, Context.User));
            }
            else
            {

                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("É...",
                        "As notificações já estão desativadas, não é necessário fazer isso novamente", EmbedMessageType.Info,
                        false, Context.User));
            }


        }

        [Command("AtivarSpam")]
        [Alias("DesbloquarSpam","ASpam")]
        [Summary(
            "Ativa novamente o serivo de mensagens privadas para notificação de eventos e informações do nosso servidor.\n" +
            ",Assim você ficará informado das nossas atualizações.")]
        [CmdCategory(Categoria = CmdCategory.Ferramentas)]
        internal async Task DesbloquarSpam()
        {
            var userAccount = AccountsMananger.GetAccount(Context.User, Context.Guild);

            if (userAccount.ReceberSpam)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("É...",
                        "As notificações já estão ativas, não é necessário fazer isso novamente", EmbedMessageType.Info,
                        false, Context.User));
            }
            else
            {
                userAccount.ReceberSpam = true;
                AccountsMananger.SaveAccounts();
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Que ótimo", "A partir de agora, você será informado dos eventos e novidades do nosso servidor.\n" +
                                                                                 $"Você pode desativar essa função digitando {Config.Bot.CmdPrefix}BloquarSpam", EmbedMessageType.Config, false, Context.User));
            }
        }
    }
}