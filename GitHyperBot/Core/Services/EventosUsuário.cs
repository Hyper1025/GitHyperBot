using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Core.Services
{
    internal class EventosUsuário
    {
        internal static Task NovoMembro(SocketGuildUser usuário)
        {
            Task.Run(async () =>
            {
                var canal = (SocketTextChannel) Global.Client.GetChannel(GuildsMannanger.GetGuild(usuário.Guild).IdChatGeral);  // Chat geral (boas vindas)

                var userAccount = AccountsMananger.GetAccount(usuário, usuário.Guild);
                var guildAccount = GuildsMannanger.GetGuild(usuário.Guild);

                if (userAccount.NumberOfWarning != 0)
                {
                    userAccount.NumberOfWarning = 0;
                    AccountsMananger.SaveAccounts();
                }

                // Log Entrou
                await Logger.ChatLogTask(Logger.LogType.UsuárioEntrou, (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(usuário.Guild).IdChatLog),usuário);

                //  Verifica se o usuário não é um BOT
                if (!usuário.IsBot)
                {
                    // Tenta enviar mensagem de boas vindas no privado
                    if (guildAccount.BoasVindasPvBool)
                    {
                        try
                        {
                            // Gera mensagem de boas vindas no privado
                            var pv = await usuário.GetOrCreateDMChannelAsync();
                            await pv.SendMessageAsync("", false,
                                EmbedHandler.CriarEmbedBoasVindas(BoasVindaType.Pv, usuário));
                        }
                        catch (Exception)
                        {
                            var pverro = await canal.SendMessageAsync("", false,
                                EmbedHandler.CriarEmbed("Erro",
                                    $"Não consegui enviar a mensagem de boas vindas no privado de {usuário.Mention}",
                                    EmbedMessageType.Error, false));
                            await Task.Delay(5000);
                            await pverro.DeleteAsync();
                        }
                    }
                }

                //  Contador de usuários no tópico do chat geral
                //  Verifica se essa função está ativa...
                if (guildAccount.TopicoChatGeralBool)
                {
                    //  Modifica o tópico do chat
                    await canal.ModifyAsync(properties =>
                    {
                        if (guildAccount.TopicoChatGeral != null)
                        {
                            properties.Topic = guildAccount.TopicoChatGeral.Replace("{contador}", ConversorDeNumerosService.NumeroParaEmoji(usuário.Guild.MemberCount.ToString()));
                        }
                    });
                }

                //Envia mensagem de boas vindas no canal de boas vindas
                if (guildAccount.BoasVindasBool)
                {
                    var m = await canal.SendMessageAsync("", false,
                        EmbedHandler.CriarEmbedBoasVindas(BoasVindaType.ChatGeral, usuário));
                    await Task.Delay(10000);
                    await m.DeleteAsync();
                }

                //  Verifica se a role do auto-role está definida
                if (guildAccount.IdRoleSemRegistro != 0)
                {
                    try
                    {
                        //  Pega a role de sem registro
                        var r = usuário.Guild.Roles.FirstOrDefault(x => x.Id == guildAccount.IdRoleSemRegistro);
                        //  Adiciona a role ao usuário
                        if (r == null)
                        {
                            //  Caso não retorne uma role
                            await canal.SendMessageAsync("", false,
                                EmbedHandler.CriarEmbed("Algo não está certo",
                                    $"Não encontrei nenhuma role de id ``{guildAccount.IdRoleSemRegistro}``.\nEssa role deveria ser referente a role do auto-role...\nSerá necessário que você redefina ela.",
                                    EmbedMessageType.Confused));
                        }
                        else
                        {
                            //  Caso retorne uma role
                            if (guildAccount.DelayAutoRole)
                            {
                                await Task.Delay(120000); //    Delay de 2 minutos
                            }
                            await usuário.AddRoleAsync(r);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
            return Task.CompletedTask;
        }

        internal static async Task UsuárioSaiu(SocketGuildUser usuário)
        {
            //  Verificamos se o usuário não foi banido
            var bans = await usuário.Guild.GetBansAsync();
            var selecionado = bans.FirstOrDefault(x => x.User.Id == usuário.Id);

            if (selecionado == null)
            {
                //  Se ele não foi banido, então enviamos que ele realmente saiu
                await Logger.ChatLogTask(Logger.LogType.UsuárioSaiu, (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(usuário.Guild).IdChatLog),usuário);
            }

            var guildAccount = GuildsMannanger.GetGuild(usuário.Guild);
            var canal = (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(usuário.Guild).IdChatGeral);  // Chat geral (boas vindas)

            if (guildAccount.TopicoChatGeralBool)
            {
                await canal.ModifyAsync(properties =>
                {
                    if (guildAccount.TopicoChatGeral != null) properties.Topic = guildAccount.TopicoChatGeral.Replace("{contador}", 
                        ConversorDeNumerosService.NumeroParaEmoji(usuário.Guild.MemberCount.ToString()));
                });
            }

        }

        internal static async Task UsuárioBanido(SocketUser usuário, SocketGuild guild)
        {
            //  Infelizmente eu não tenho como obter o autor do ban dessa forma, somente pelo audit log
            //  Coisa que o discord ainda não libera para os dev's... T_T
            //  Então, para no log ter todos os bans, não terá mais o autor do ban no log

            var bans = await guild.GetBansAsync();
            var selecionado = bans.FirstOrDefault(x => x.User.Id == usuário.Id);

            if (selecionado != null)
            {
                await Logger.ChatLogTask(Logger.LogType.UsuárioBanido, (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(guild).IdChatLog), usuário as SocketGuildUser, selecionado.Reason);
            }
            else
            {
                await Logger.ChatLogTask(Logger.LogType.UsuárioBanido, (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(guild).IdChatLog), usuário as SocketGuildUser);
            }

            try
            {
                await usuário.SendMessageAsync("Tenho uma má notícia...", false,
                    EmbedHandler.CriarEmbedBanPv(usuário, guild, selecionado?.Reason ?? "Sem motivo informado"));
            }
            catch (Exception)
            {
                await Logger.ConsoleLogComGuild(guild, usuário.Username, $"Erro notificação privada de banimento", ConsoleColor.Yellow);
                throw;
            }
        }

        internal static async Task UsuárioDesbanido(SocketUser usuário, SocketGuild guild)
        {
            //  Define variaiveis
            var emb = new EmbedBuilder();
            var canal = (SocketTextChannel)guild.GetChannel(GuildsMannanger.GetGuild(guild).IdChatLog);

            //  Cria a embed
            emb.WithTitle(":handshake: Desbanido")
                .WithColor(new Color(123, 123, 213))
                .WithFooter($"ID do usuário:{usuário.Id}")
                .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                .WithCurrentTimestamp();
            //  Envia
            await canal.SendMessageAsync("", false, emb);
        }
    }
}