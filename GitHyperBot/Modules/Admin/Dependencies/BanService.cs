using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules.Admin.Dependencies
{
    public class BanService
    {
        // TODO: Implementações no sistema de ban (Gifs)

        internal static async Task BanirUsuario(SocketGuildUser usuárioBanido, SocketGuildUser moderador, SocketGuild guild, SocketTextChannel canal, string razão)
        {
            try
            {
                await usuárioBanido.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Você foi banido",
                        $"Você foi banido de {guild.Name}, por {moderador.Mention}, em razão de:\n```{razão}```", EmbedMessageType.Error, true, usuárioBanido));
            }
            catch (Exception)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Erro...", "Não foi possível enviar mensagem no chat do usuário.",
                        EmbedMessageType.Error, false));
            }

            await guild.AddBanAsync(usuárioBanido, 0, razão);

            var m = await canal.SendMessageAsync("", false,
                EmbedHandler.CriarEmbedBan(moderador,usuárioBanido,razão,false));
            await Task.Delay(10000);
            await m.DeleteAsync();
        }
    }
}