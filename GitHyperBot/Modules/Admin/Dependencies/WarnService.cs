using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Core.Services;

namespace GitHyperBot.Modules.Admin.Dependencies
{
    public class WarnService
    {
        internal static async Task AdicionarWarnTask(SocketGuildUser usuário,SocketGuildUser moderador ,ISocketMessageChannel canal)
        {
            if (usuário.IsBot)return;

            var userAccount = AccountsMananger.GetAccount(usuário, usuário.Guild);
            var guildAccount = GuildsMannanger.GetGuild(usuário.Guild);

            userAccount.NumberOfWarning++;
            AccountsMananger.SaveAccounts();

            if (userAccount.NumberOfWarning >= guildAccount.MaxWarning)
            {
                await BanService.BanirUsuario(usuário, moderador, usuário.Guild, (SocketTextChannel) canal,
                    $"Completar as {guildAccount.MaxWarning} advertências necessárias para ser banido");
            }
            else
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Advertido", $"O usuário {usuário.Mention} levou uma advertência",
                        EmbedMessageType.Warning));
                try
                {
                    await usuário.SendMessageAsync("", false,
                        EmbedHandler.CriarEmbed("Advertido", $"Infelizmente você levou um warn.\n\nvocê tem **{userAccount.NumberOfWarning} advertências**, ao chegar em **{guildAccount.MaxWarning}** você será banido.", EmbedMessageType.Error));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                await Logger.ChatLogTask(Logger.LogType.Warn,
                    (SocketTextChannel) Global.Client.GetChannel(GuildsMannanger.GetGuild(usuário.Guild).IdChatLog),
                    usuário);
            }
        }

        internal static async Task TirarWarnTask(SocketGuildUser usuário, ISocketMessageChannel canal, uint quantidade)
        {
            if (usuário.IsBot) return;

            var userAccount = AccountsMananger.GetAccount(usuário, usuário.Guild);

            if (quantidade > userAccount.NumberOfWarning)
            {
                quantidade = userAccount.NumberOfWarning;           
            }

            await canal.SendMessageAsync("", false,
                EmbedHandler.CriarEmbed("Certo", $"Removi {quantidade} dos {userAccount.NumberOfWarning} warns ",
                    EmbedMessageType.Success, false));

            userAccount.NumberOfWarning = userAccount.NumberOfWarning - quantidade;
            AccountsMananger.SaveAccounts();
        }

        internal static uint WarnsTask(SocketGuildUser usuário)
        {
            var userAcount = AccountsMananger.GetAccount(usuário, usuário.Guild);

            return userAcount.NumberOfWarning;
        }
    }
}