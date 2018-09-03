using System;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.Server;

namespace GitHyperBot.Core.Handlers
{
    public class EmbedHandler
    {
        internal static Embed CriarEmbed(string título, string descrição, EmbedMessageType tipo, bool comTempo = true, SocketUser usuário = null, bool comAutor = true)
        {
            var embed = new EmbedBuilder();

            embed.WithDescription(descrição);

            switch (tipo)
            {
                case EmbedMessageType.Info:
                    embed.WithTitle($":information_source: {título}")
                        .WithColor(new Color(52, 152, 219));
                    break;
                case EmbedMessageType.Success:
                    embed.WithTitle($":white_check_mark: {título}")
                        .WithColor(new Color(22, 160, 133));
                    break;
                case EmbedMessageType.Error:
                    embed.WithTitle($":worried: {título}")
                        .WithColor(new Color(192, 57, 43));
                    break;
                case EmbedMessageType.Confused:
                    embed.WithTitle($":thinking: {título}")
                        .WithColor(new Color(255, 191, 0));
                    break;
                case EmbedMessageType.AccessDenied:
                    embed.WithTitle($":no_entry_sign: {título}").
                        WithColor(new Color(192, 57, 43));
                    break;
                case EmbedMessageType.Warning:
                    embed.WithTitle($":warning: {título}")
                        .WithColor(new Color(255, 191, 0));
                    break;
                case EmbedMessageType.GoldGain:
                    embed.WithTitle($":dollar: {título}")
                        .WithColor(new Color(244, 217, 66));
                    break;
                case EmbedMessageType.Config:
                    embed.WithTitle($":wrench: {título}").
                        WithColor(new Color(188, 188, 188));
                    break;
                case EmbedMessageType.GoldLose:
                    embed.WithTitle($":money_with_wings: {título}").
                        WithColor(new Color(244, 134, 66));
                    break;
                default:
                    embed.WithColor(new Color(178, 178, 178));
                    break;
            }
            
            if (usuário != null && comAutor)
            {
                var r = new Random();
                embed.WithAuthor(usuário.Username,
                    usuário.GetAvatarUrl() ?? $"https://cdn.discordapp.com/embed/avatars/{r.Next(0, 4)}.png");
            }

            if (comTempo)
            {
                embed.WithCurrentTimestamp();
            }

            return embed;
        }

        internal static Embed CriarEmbedComImagem(string titulo, string imagem, string descrição = null, bool comTempo = false, string rodapé = null, string imagemRodapé = null)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(titulo).WithImageUrl(imagem);
            if (descrição != null)
            {
                embed.WithDescription(descrição);
            }

            if (rodapé != null && imagemRodapé != null)
            {
                embed.WithFooter(rodapé, imagemRodapé);
            }
            else if (rodapé != null)
            {
                embed.WithFooter(rodapé);
            }

            if (comTempo)
            {
                embed.WithCurrentTimestamp();
            }

            var r = new Random();
            embed.WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)));

            return embed;
        }
        
        internal static Embed CriarEmbedBoasVindas(BoasVindaType tipo, SocketGuildUser usuário)
        {
            var emb = new EmbedBuilder();
            var guildAccount = GuildsMannanger.GetGuild(usuário.Guild);
            var r = new Random();

            switch (tipo)
            {
                #region ChatGeral
                case BoasVindaType.ChatGeral:
                    //  Título
                    if (guildAccount.BoasVindasTitle != null) emb.WithTitle(guildAccount.BoasVindasTitle.Replace("{mencionar}", usuário.Mention));

                    //  Descriçãp
                    if (guildAccount.BoasVindasDescription != null)
                        emb.WithDescription(guildAccount.BoasVindasDescription.Replace("{mencionar}", usuário.Mention));

                    //  Imagem
                    if (guildAccount.BoasVindasUrl != null)
                        emb.WithImageUrl(guildAccount.BoasVindasUrl);

                    //  Tumb do usuário
                    if (guildAccount.BoasVindasTumbUsuario)
                        emb.WithThumbnailUrl(usuário.GetAvatarUrl());

                    //  Data de criação da conta
                    emb.AddField("Conta criada em",
                        $"```fix\n{usuário.CreatedAt.Day}/{usuário.CreatedAt.Month}/{usuário.CreatedAt.Year}```");
                    break;
                #endregion

                #region Privado

                case BoasVindaType.Pv:
                    //  Título
                    if (guildAccount.BoasVindasPvTitle != null)
                        emb.WithTitle(guildAccount.BoasVindasPvTitle.Replace("{mencionar}", usuário.Mention));

                    //  Descrição
                    if (guildAccount.BoasVindasPvDescription != null)
                        emb.WithDescription(guildAccount.BoasVindasPvDescription.Replace("{mencionar}", usuário.Mention));

                    //  Imagem
                    if (guildAccount.BoasVindasPvUrl != null)
                        emb.WithImageUrl(guildAccount.BoasVindasPvUrl);

                    //  Tumb do usuário
                    if (guildAccount.BoasVindasPvTumbUsuario)
                        emb.WithThumbnailUrl(usuário.GetAvatarUrl());

                    //  Campo 1
                    if (guildAccount.BoasVindasPvField1Title != null && guildAccount.BoasVindasPvField1Descri != null)
                        emb.AddField(guildAccount.BoasVindasPvField1Title.Replace("{mencionar}", usuário.Mention), guildAccount.BoasVindasPvField1Descri.Replace("{mencionar}", usuário.Mention));

                    //  Campo 2
                    if (guildAccount.BoasVindasPvField2Title != null && guildAccount.BoasVindasPvField2Descri != null)
                        emb.AddField(guildAccount.BoasVindasPvField2Title.Replace("{mencionar}", usuário.Mention), guildAccount.BoasVindasPvField2Descri.Replace("{mencionar}", usuário.Mention));

                    //  Campo 3
                    if (guildAccount.BoasVindasPvField3Title != null && guildAccount.BoasVindasPvField3Descri != null)
                        emb.AddField(guildAccount.BoasVindasPvField3Title.Replace("{mencionar}", usuário.Mention), guildAccount.BoasVindasPvField3Descri.Replace("{mencionar}", usuário.Mention));

                    //  Rodapé
                    if (guildAccount.BoasVindasPvFooter != null)
                        emb.WithFooter(guildAccount.BoasVindasPvFooter.Replace("{mencionar}", usuário.Mention));
                    break;

                    #endregion
            }
            emb.WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)));

            return emb;
        }

        internal static Embed CriarEmbedPerfilDivulgador(SocketGuildUser usuário, uint level, uint usos)
        {
            var r = new Random();
            var emb = new EmbedBuilder();

            emb.WithAuthor(usuário.Username,
                    usuário.GetAvatarUrl() ?? $"https://cdn.discordapp.com/embed/avatars/{r.Next(0, 4)}.png")
                .WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)))
                .WithThumbnailUrl(usuário.GetAvatarUrl() ??
                                  $"https://cdn.discordapp.com/embed/avatars/{usuário.DiscriminatorValue % 5}.png")
                .AddInlineField("Usuário", $"```md\n[id]({usuário.Id.ToString()})```")
                .AddInlineField("Nível", $"```fix\n{level}```")
                .AddInlineField("Convidados", $"```fix\n{usos}```");

            return emb;
        }

        internal static Embed CriarEmbedBan(SocketGuildUser adm, SocketGuildUser usuário, string razão, bool joke)
        {
            var emb = new EmbedBuilder();
            var gifSelecionado = GifsBans[Global.Rng.Next(0, GifsBans.Length)];

            emb.WithTitle("🚫 Banido")
                .WithDescription($"{adm.Mention}, baniu ")
                .WithDescription($"O usuário {usuário.Mention} foi banido.\n" +
                                 "Razão:\n" +
                                 $"```{razão}```");

            emb.WithColor(joke ? new Color(66, 134, 244) : new Color(244, 66, 66));

            emb.WithThumbnailUrl(usuário.GetAvatarUrl() ??
                                 $"https://cdn.discordapp.com/embed/avatars/{usuário.DiscriminatorValue % 5}.png")
                .WithFooter(adm.Username, adm.GetAvatarUrl())
                .WithCurrentTimestamp()
                .WithImageUrl(gifSelecionado);

            return emb;
        }

        internal static Embed CriarEmbedBanPv(SocketUser usuário, SocketGuild guild, string razão)
        {
            var r = new Random();
            var emb = new EmbedBuilder();

            emb.WithTitle("Notificação de banimento")
                .WithDescription(
                    $"Olá, {usuário.Mention}, vim lhe informar de que você foi banido de {guild.Name} pelo seguinte motivo\n```diff\n-{razão}```")
                .WithThumbnailUrl(usuário.GetAvatarUrl() ?? $"https://cdn.discordapp.com/embed/avatars/{usuário.DiscriminatorValue % 5}.png")
                .WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)))
                .WithCurrentTimestamp();

            return emb;
        }

        internal static string[] GifsBans =
        {
            "https://i.imgur.com/nPsetVZ.gif",
            "https://i.imgur.com/O3DHIA5.gif",
            "https://i.imgur.com/aspwN41.gif",
            "https://i.imgur.com/wOwknGW.gif",
            "https://media.giphy.com/media/qPD4yGsrc0pdm/giphy.gif",
            "https://media.giphy.com/media/C51woXfgJdug/giphy.gif",
            "https://i.imgur.com/Z2klJ5W.gif",
            "https://media.giphy.com/media/MEw0inp5gAlzO/giphy.gif",
            "https://media.giphy.com/media/825PVJzyHP03K/giphy.gif",
            "https://i.imgur.com/cwDETTU.gif"
        };

    }

    internal enum EmbedMessageType
    {
        Success,
        Info,
        Error,
        GoldLose,
        Confused,
        AccessDenied,
        Warning,
        GoldGain,
        Config
    }

    internal enum BoasVindaType
    {
        ChatGeral,
        Pv
    }
}