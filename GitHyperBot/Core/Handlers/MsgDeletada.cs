using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Services;
using SocketGuildUser = Discord.WebSocket.SocketGuildUser;

namespace GitHyperBot.Core.Handlers
{
    public static class MsgDeletada
    {
        internal static async Task MensagemDeletada(Cacheable<IMessage, ulong> mensagem, ISocketMessageChannel canal)
        {
            var msg = mensagem.GetOrDownloadAsync().Result;
            var user = (SocketGuildUser)msg.Author;
            var idLogCanal = GuildsMannanger.GetGuild(user.Guild).IdChatLog;

            var arquivo = msg.Attachments.FirstOrDefault();

            await Logger.ChatLogEventosMensagens(Logger.LogTypeEventosMensagens.MsgDeletada,
                (SocketTextChannel) Global.Client.GetChannel(idLogCanal), (SocketUser) msg.Author,
                (SocketTextChannel) msg.Channel, msg.Content, arquivo?.Url);
        }
    }
}