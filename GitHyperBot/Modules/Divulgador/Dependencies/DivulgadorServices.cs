using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Core.Services;

namespace GitHyperBot.Modules.Divulgador.Dependencies
{
    public class DivulgadorServices
    {
        internal static async Task DivulgadorTask(SocketGuildUser usuário, SocketTextChannel canal)
        {
            var convites = await usuário.Guild.GetInvitesAsync();
            var usuárioSelecionado = convites.FirstOrDefault(x => x.Inviter.Id == usuário.Id);
            var usos = 0;

            if (usuárioSelecionado == null)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...", $"O usuário {usuário.Mention} nunca gerou um convite.",
                        EmbedMessageType.Error, false));
                return;
            }

            if (usuárioSelecionado.Url == null)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...", $"Não encontrei convites ativos para o usuário {usuário.Mention}.",
                        EmbedMessageType.Error, false));
                return;
            }

            //foreach (var aux in convites.Where(x=>x.Inviter.Id == usuário.Id))
            //{
            //    usos += aux.Uses;
            //}

            usos += convites.Where(x => x.Inviter.Id == usuário.Id).Sum(y => y.Uses);
            
            await canal.SendMessageAsync("", false, EmbedHandler.CriarEmbedPerfilDivulgador(usuário, ExpService.CalcularNivel(usos), Convert.ToUInt32(usos)));

        }

        internal static async Task Convites(SocketGuildUser usuário, SocketTextChannel canal)
        {
            var convites = await usuário.Guild.GetInvitesAsync();
            var usuárioSelecionado = convites.FirstOrDefault(x => x.Inviter.Id == usuário.Id);

            if (usuárioSelecionado == null)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...", $"O usuário {usuário.Mention} nunca gerou um convite.",
                        EmbedMessageType.Error, false));
                return;
            }

            if (usuárioSelecionado.Url == null)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...", $"Não encontrei convites ativos para o usuário {usuário.Mention}.",
                        EmbedMessageType.Error, false));
                return;
            }

            var r = new Random();
            var emb = new EmbedBuilder();

            emb.WithAuthor(usuário.Username,
                    usuário.GetAvatarUrl() == null
                        ? $"https://cdn.discordapp.com/embed/avatars/{usuário.DiscriminatorValue % 5}.png"
                        : usuário.GetAvatarUrl())
                .WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)))
                .WithThumbnailUrl(usuário.GetAvatarUrl() ??
                                  $"https://cdn.discordapp.com/embed/avatars/{usuário.DiscriminatorValue % 5}.png")
                .WithDescription("Aqui estão os seus convites");

            foreach (var metadata in convites.Where(x=>x.Inviter.Id == usuário.Id))
            {
                emb.AddField(metadata.Url, $"Usos: {metadata.Uses}");
            }

            await canal.SendMessageAsync("", false, emb);
        }

        internal static async Task TopInviters(SocketGuildUser usuário, SocketTextChannel canal)
        {
            var convites = await usuário.Guild.GetInvitesAsync();

            var emb = new EmbedBuilder();

            if (convites.Count < 5)
            {
                await canal.SendMessageAsync("", false, EmbedHandler.CriarEmbed("É...","Sua guild não o mínimo de 5 convites gerados, tente novamente quando tiver mais.", EmbedMessageType.Info));
            }

            var agruoadoList = convites.OrderByDescending(x=>x.Uses).GroupBy(x => x.Inviter.Id).Take(5).ToList();

            var n = 0;
            var total = 0;

            while (n <= 4)
            {
                var selecionado = agruoadoList.ElementAt(n).FirstOrDefault();
                var usos = convites.Where(x => selecionado != null && x.Inviter.Id == selecionado.Inviter.Id).Sum(aux => aux.Uses);
                var emoji = "";
                total = total + usos;

                switch (n)
                {
                    case 0:
                        emoji = ":first_place:";
                        break;
                    case 1:
                        emoji = ":second_place:";
                        break;
                    case 2:
                        emoji = ":third_place:";
                        break;
                    case 3:
                        emoji = "<:Medalha4:461384742114295808>";
                        break;
                    case 4:
                        emoji = "<:Medalha5:461384754131107850>";
                        break;
                }
                if (selecionado != null) emb.AddField($"{emoji} - {selecionado.Inviter.Username}", $"\n_Convidou_```autohotkey\nConvidou: {usos}```");
                n++;
            }

            emb.WithTitle("Top Divulgadores")
                .WithThumbnailUrl(usuário.Guild.IconUrl)
                .WithCurrentTimestamp().WithFooter($"Total: {total}",Global.Client.CurrentUser.GetAvatarUrl());

            await canal.SendMessageAsync("", false, emb);
        }
    }
}