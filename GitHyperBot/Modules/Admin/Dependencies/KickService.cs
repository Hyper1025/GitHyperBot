using System.Threading.Tasks;
using Discord.WebSocket;

namespace GitHyperBot.Modules.Admin.Dependencies
{
    public class KickService
    {
        //  Todo: Comando e serviço kick

        public static async Task KickarUsuário(SocketGuildUser kickado, SocketTextChannel canal = null, string razão = null)
        {
            await kickado.KickAsync();
        }
    }
}