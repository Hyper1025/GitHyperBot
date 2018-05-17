using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules
{
    public class GuildConfig : ModuleBase<SocketCommandContext>
    {
        [Group("Definir"), Summary("Configuração da guild")]    //  Criamos um grupo para definir configurações
        [RequireUserPermission(GuildPermission.Administrator)]  //  Requerimos permissão de adm para usar esse grupo de cmd's
        public class ConfigGruoup : ModuleBase<SocketCommandContext>
        {
            [Command("ChatGeral")]
            public async Task DefinirChatGeralTask()
            {
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                guild.IdChatGeral = Context.Channel.Id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("",false,EmbedHandler.CriarEmbed
                    ("Definido",$"Canal de texto: ``{Context.Channel.Name}``\nId: ``{Context.Channel.Id}``\nFoi definido como **chat geral**", EmbedMessageType.Success));
            }
        }
    }
}