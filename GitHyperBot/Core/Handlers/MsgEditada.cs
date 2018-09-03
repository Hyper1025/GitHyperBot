using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Services;

namespace GitHyperBot.Core.Handlers
{
    public static class MsgEditada
    {
        internal static async Task MensagemEditada(Cacheable<IMessage, ulong> mensagemAntiga, SocketMessage mensagemEditada,ISocketMessageChannel canalDaMensagem)
        {
            if (mensagemEditada.Author.IsBot)
            {
                return;
            }
            //throw new NotImplementedException();
            var msgAntigaResult = mensagemAntiga.GetOrDownloadAsync().Result;
            var user = (SocketGuildUser)mensagemEditada.Author;
            var idLogCanal = GuildsMannanger.GetGuild(user.Guild).IdChatLog;

            await Logger.ChatLogEventosMensagens(Logger.LogTypeEventosMensagens.MsgEditada,
                (SocketTextChannel) Global.Client.GetChannel(idLogCanal), mensagemEditada.Author,
                (SocketTextChannel) mensagemEditada.Channel, msgAntigaResult.Content,
                msgAntigaResult.Attachments.FirstOrDefault()?.Url, mensagemEditada.Content);
        }
    }
}