using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.Server;

namespace GitHyperBot.Core.Handlers
{
    public class ReactionsHandler
    {
        internal static async Task ReaçãoAdicionada(Cacheable<IUserMessage, ulong> mensagem, ISocketMessageChannel canal, SocketReaction reação)
        {
            //  Obtemos a conta da guild
            if (reação.User.Value.IsBot)return;
            var guildChanel = (SocketGuildChannel) canal;
            var guild = guildChanel.Guild;
            var guildAccount = GuildsMannanger.GetGuild(guild);

            //  Verificamos se a reação foi na mensasgem
            if (reação.MessageId != guildAccount.IdMsgRegras) return;

            //  Obtemos o usuário
            var usuário = (SocketGuildUser)reação.User;

            //  Obtemos a role
            var role = guild.Roles.FirstOrDefault(x => x.Id == guildAccount.IdRoleRegras);

            //  Verificamos o emoji
            if (reação.Emote.Name == "✅")
            {
                //  Adicionamos a role ao usuário
                await usuário.AddRoleAsync(role);
                //  Agradecemos o usuário
                var pv = await usuário.GetOrCreateDMChannelAsync();
                await pv.SendMessageAsync($"Obrigado por aceitar as regras do nosso servidor.\nAgora você tem acesso ao servidor, sempre que quiser.\n\nAtenciosamente toda equipe do **{guild.Name}**");
            }
        }
    }
}