using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Core.Services;

namespace GitHyperBot.Modules.Admin.Dependencies
{
    public class MuteService
    {
        internal static async Task MutarTask(SocketGuildUser usuário, ISocketMessageChannel canal)
        {
            //  Setamoms o nome da role
            var roleName = $"{Global.Client.CurrentUser.Username}-Mute";

            //  Verificamos se a role existe
            if (usuário.Guild.Roles.All(x => x.Name != roleName))
            {
                //  Se não existir, criamos
                await usuário.Guild.CreateRoleAsync(roleName,
                    new GuildPermissions(), Color.Magenta);
            }

            //  Selecionamos a role
            var roleSelecionada = usuário.Guild.Roles.FirstOrDefault(x => x.Name == roleName);

            //  Atribuimos como falsa a permissão de enviar msg's e reagir em todos os chats da guild
            foreach (var auxCanal in usuário.Guild.Channels)
            {
                try
                {
                    if (auxCanal.PermissionOverwrites.Any(x =>
                    roleSelecionada != null && (x.TargetType == PermissionTarget.Role && x.TargetId == roleSelecionada.Id &&
                                                x.Permissions.Equals(new OverwritePermissions(sendMessages: PermValue.Deny,
                                                    addReactions: PermValue.Deny, attachFiles: PermValue.Deny)))))
                        continue;
                }
                catch (Exception)
                {
                    //Console.WriteLine(e);
                }

                try
                {
                    await auxCanal.AddPermissionOverwriteAsync(
                        usuário.Guild.Roles.FirstOrDefault(x => x.Name == roleName),
                        new OverwritePermissions(sendMessages: PermValue.Deny, addReactions: PermValue.Deny, attachFiles:PermValue.Deny));
                }
                catch (Exception)
                {
                    //Console.WriteLine(e);
                }
            }

            //  Verificamos se o usuário já tem a role
            if (usuário.Roles.Contains(roleSelecionada))
            {
                //  Caso tenha, retornamos uma mensagem, e saimos do método
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...", "O usuário já está mutado", EmbedMessageType.Error,false));
                return;
            }

            //  Adicionamoos a role
            await usuário.AddRoleAsync(roleSelecionada);
            //  Avisamos no chat do mute
            await canal.SendMessageAsync("", false,
                EmbedHandler.CriarEmbed("Mutado...", $"O usuário {usuário.Mention} foi mutado",
                    EmbedMessageType.Success, false));

            //  Informamos no privado do usuário
            try
            {
                await usuário.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Mutado", "Infelizmente você foi mutado", EmbedMessageType.Error));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            //  Enviamos o log
            await Logger.ChatLogTask(Logger.LogType.UsuárioMutado,
                (SocketTextChannel) Global.Client.GetChannel(GuildsMannanger.GetGuild(usuário.Guild).IdChatLog),
                usuário);
        }

        internal static async Task DesmutarTask(SocketGuildUser usuário, ISocketMessageChannel canal)
        {
            var roleName = $"{Global.Client.CurrentUser.Username}-Mute";
            var roleSelecionada = usuário.Guild.Roles.FirstOrDefault(x => x.Name == roleName);

            if (!usuário.Roles.Contains(roleSelecionada))
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...", "O usuário já está desmutado", EmbedMessageType.Error, false));
                return;
            }

            await usuário.RemoveRoleAsync(roleSelecionada);
            await canal.SendMessageAsync("", false,
                EmbedHandler.CriarEmbed("Desmutado...", $"O usuário {usuário.Mention} foi desmutado",
                    EmbedMessageType.Success, false));

            try
            {
                await usuário.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Desmutado", "Você foi desmutado", EmbedMessageType.Info));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            await Logger.ChatLogTask(Logger.LogType.UsuárioDesmutado,
                (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(usuário.Guild).IdChatLog),
                usuário);
        }
    }
}