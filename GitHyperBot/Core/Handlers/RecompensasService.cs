using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Services;

namespace GitHyperBot.Core.Handlers
{
    internal static class RecompensasService
    {
        internal static async Task HandleMsgRewards(SocketMessage s)
        {

            var msg = (SocketUserMessage) s;
            var guild = (SocketGuildUser) s.Author; 

            if (msg == null) return;
            if (msg.Author.IsBot) return;
            if (msg.Channel == await msg.Author.GetOrCreateDMChannelAsync()) return;
            
            var userAccount = AccountsMananger.GetAccount(msg.Author, guild.Guild);
            var agora = DateTime.UtcNow;

            if (agora < userAccount.UltimaMensagem.AddSeconds(20) || msg.Content.Length < 10) return;
            var r = new Random();
            var levelAntigo = ExpService.CalcularNivel((int)userAccount.Xp);

            userAccount.Xp += (ulong) r.Next(1, 5);
            userAccount.Gold += (ulong)r.Next(1, 10);
            userAccount.UltimaMensagem = agora;

            AccountsMananger.SaveAccounts();

            var levelNovo = ExpService.CalcularNivel((int)userAccount.Xp);

            if (levelNovo > levelAntigo)
            {
                await s.Channel.SendMessageAsync("", false, EmbedHandler.CriarEmbed("BOA!",
                    $"Você subiu de nível, de {levelAntigo} para {levelNovo}", EmbedMessageType.Success, false
                    , msg.Author));
            }
        }
    }
}