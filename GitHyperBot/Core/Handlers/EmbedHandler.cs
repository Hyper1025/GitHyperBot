using Discord;
using Discord.WebSocket;

namespace GitHyperBot.Core.Handlers
{
    public class EmbedHandler
    {
        public static Embed CriarEmbed(string title, string body, EmbedMessageType type, bool withTimeStamp = true, SocketUser user = null, bool withAuthor = true)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(body);

            switch (type)
            {
                case EmbedMessageType.Info:
                    embed.WithColor(new Color(52, 152, 219));
                    embed.WithThumbnailUrl("http://www.hey.fr/fun/emoji/android/en/icon/android/20-emoji_android_information_source.png");
                    break;
                case EmbedMessageType.Success:
                    embed.WithColor(new Color(22, 160, 133));
                    embed.WithThumbnailUrl("http://www.emoji.co.uk/files/twitter-emojis/symbols-twitter/11160-white-heavy-check-mark.png");
                    break;
                case EmbedMessageType.Error:
                    embed.WithColor(new Color(192, 57, 43));
                    embed.WithThumbnailUrl("https://cdn.iconscout.com/public/images/icon/free/png-512/dizzy-face-cross-error-emoji-3a5cd2ef4699d800-512x512.png");
                    break;
                case EmbedMessageType.Exception:
                    embed.WithColor(new Color(230, 126, 34));
                    break;
                case EmbedMessageType.Confused:
                    embed.WithColor(new Color(255, 191, 0));
                    embed.WithThumbnailUrl("https://i.imgur.com/QpG1cTI.gif");
                    //embed.WithThumbnailUrl("https://media3.giphy.com/media/CaiVJuZGvR8HK/giphy.gif");
                    break;
                case EmbedMessageType.AccessDenied:
                    embed.WithColor(new Color(192, 57, 43));
                    embed.WithThumbnailUrl("https://vignette.wikia.nocookie.net/undertale-rp/images/0/0d/Access_Denied.png");
                    break;
                case EmbedMessageType.Warning:
                    embed.WithColor(new Color(255, 191, 0));
                    embed.WithThumbnailUrl("https://images.emojiterra.com/twitter/512px/26a0.png");
                    break;
                case EmbedMessageType.GoldGain:
                    embed.WithColor(new Color(244, 217, 66));
                    embed.WithThumbnailUrl("https://i.imgur.com/AT7G9N2.png");
                    break;
                case EmbedMessageType.Config:
                    embed.WithColor(new Color(188, 188, 188));
                    embed.WithThumbnailUrl("https://images.emojiterra.com/twitter/512px/2699.png");
                    break;
                default:
                    embed.WithColor(new Color(178, 178, 178));
                    break;
            }

            if (user != null && withAuthor)
            {
                embed.WithAuthor(user.Username, user.GetAvatarUrl());
            }

            if (withTimeStamp)
            {
                embed.WithCurrentTimestamp();
            }

            return embed;
        }
    }

    public enum EmbedMessageType
    {
        Success = 0,
        Info = 1,
        Error = 2,
        Exception = 3,
        Confused = 4,
        AccessDenied = 5,
        Warning = 6,
        GoldGain = 7,
        Config = 8,
    }
}