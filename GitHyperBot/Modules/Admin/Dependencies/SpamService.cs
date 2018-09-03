using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Admin.Dependencies.json;

namespace GitHyperBot.Modules.Admin.Dependencies
{
    public class SpamService
    {
        internal static Task SpamPrivado(SocketGuildUser mod, string json, SocketTextChannel canal)
        {
            Task.Run(async () =>
            {
                //  Atribuimos os usuários da guild a uma variável
                var usuarios = mod.Guild.Users;
                //  Lemos o json
                var spamMessage = SpamMessage.FromJson(json);
                //  Usando o json enviado pelo usuário
                if (spamMessage.ImagemUrl.ToLower().Contains(".jpg") || spamMessage.ImagemUrl.ToLower().Contains(".png") || spamMessage.ImagemUrl.ToLower().Contains(".gif"))
                {
                    
                }
                else
                {
                    await canal.SendMessageAsync("", false,
                        EmbedHandler.CriarEmbed("Erro na imagem", $"O link: ``{spamMessage.ImagemUrl}`` não corresponde a uma imagem própriamente dita," +
                                                                  " por favor, tenha certeza de que o link termine com **.jpg, .png ou .gif**", EmbedMessageType.Error, false));
                    return;
                }

                //  Criamos uma var para armazenar o número de usuários atingidos pela mensagem
                uint spamados = 0;
                //  Criamos uma var para atribuir o número total de usuários na guild
                var total = (uint)mod.Guild.MemberCount;
                uint ignorarSpam = 0;
                //  Enviamos no canal onde o comando foi executado a informação de que estamos iniciando o spam
                await canal.SendMessageAsync($"Iniciando spam...\n{spamMessage.Mensagem.Replace("{mencionar}", mod.Mention)}", false, RetornarEmbedSpam(mod, json, mod));

                //  Percorremos os usuários da guild, enviando a msg para cada um deles
                foreach (var guildUser in usuarios.Where(aux=>aux.IsBot == false))
                {
                    try
                    {
                        //  Obtemos a conta do usuário
                        var account = AccountsMananger.GetAccount(guildUser, guildUser.Guild);
                        //  Verificamos se ele pretende obter as mensagens
                        if (account.ReceberSpam)
                        {
                            await guildUser.SendMessageAsync(spamMessage.Mensagem.Replace("{mencionar}", guildUser.Mention), false, RetornarEmbedSpam(mod, json, guildUser));
                            spamados++;
                        }
                        else
                        {
                            ignorarSpam++;
                        }
                    }
                    catch (Exception)
                    {
                        await canal.SendMessageAsync($"Não foi possível enviar mensagem para: {guildUser.Mention}");
                    }
                }

                //  Ao concluir, informamos no chat onde o comando foi executado
                await canal.SendMessageAsync(mod.Mention, false,
                    EmbedHandler.CriarEmbed("Pronto", $"Conclui o spam com sucesso, enviado para ``{spamados}`` usuários de ``{total}`` usuários\n" +
                                                      $"Total erros: ``{total - spamados}``\n" +
                                                      $"Usuários ignorados: ``{ignorarSpam}``",
                        EmbedMessageType.Success, false));
            });
            return Task.CompletedTask;
        }

        internal static Embed RetornarEmbedSpam(SocketGuildUser mod, string json, SocketGuildUser usuário)
        {
            //  Obtemos os dados do json
            var spamMessage = SpamMessage.FromJson(json);

            //  Criamos a embed
            var r = new Random();
            var emb = new EmbedBuilder();

            //  Passamos os dados a ela
            if (spamMessage.Titulo != null) emb.WithTitle(spamMessage.Titulo.Replace("{mencionar}",usuário.Mention));
            if (spamMessage.Descrição != null) emb.WithDescription(spamMessage.Descrição.Replace("{mencionar}", usuário.Mention));
            if (spamMessage.ImagemUrl != null) emb.WithImageUrl(spamMessage.ImagemUrl);
            emb.WithAuthor(mod.Username,
                    mod.GetAvatarUrl() ?? $"https://cdn.discordapp.com/embed/avatars/{r.Next(0, 4)}.png")
                .WithFooter($"Desativar recebimento. Use {Config.Bot.CmdPrefix}BSpam, em nosso server",
                    Global.Client.CurrentUser.GetAvatarUrl())
                .WithThumbnailUrl(mod.Guild.IconUrl);

            emb.WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255))).WithCurrentTimestamp();

            return emb;
        }
    }
}