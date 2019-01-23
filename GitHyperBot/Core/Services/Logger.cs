using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace GitHyperBot.Core.Services
{
    //  <summary>
    //  Classe responsável por gerar o log que é visível no console
    //  Futuramente também irá enviar as mensagens para o chat de log
    //  de eventos do servidor
    //  </summary>

    internal class Logger
    {
        internal static Task ConsoleLog(LogMessage logMessage)
        {
            Console.ForegroundColor = SeverityConsoleColor(logMessage.Severity);
            //var message = string.Concat(DateTime.Now.ToShortTimeString(), " [", logMessage.Source, "] ", logMessage.Message);
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} [ {logMessage.Source} ] {logMessage.Message}");
            Console.ResetColor();
            return Task.CompletedTask;
        }

        internal static Task ConsoleLogComGuild(SocketGuild guild, string username, string comando, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} <{guild.Name}> [{username}] {comando}");
            Console.ResetColor();
            return Task.CompletedTask;
        }

        internal static async Task ChatLogTask(LogType logType,SocketTextChannel canalLog, SocketUser usuário, string motivo = null, string urlFile = null)
        {
            var emb = new EmbedBuilder();

            switch (logType)
            {
                case LogType.UsuárioEntrou:
                    emb.WithTitle(":point_right: Entrou")
                        .WithColor(new Color(66, 176, 244))
                        .WithFooter($"ID do usuário:{usuário.Id}")
                        .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                        .WithCurrentTimestamp();
                    break;
                case LogType.UsuárioSaiu:
                    emb.WithTitle(":point_left: Saiu")
                        .WithColor(new Color(212, 66, 244))
                        .WithFooter($"ID do usuário:{usuário.Id}")
                        .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                        .WithCurrentTimestamp();
                    break;
                case LogType.UsuárioBanido:
                    emb.WithTitle(":no_entry: Banido")
                        .WithColor(new Color(244, 65, 65));
                     if (motivo != null)
                         emb.WithDescription($"```{motivo}```");
                         emb.WithFooter($"ID do usuário:{usuário.Id}")
                            .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                            .WithCurrentTimestamp();
                    break;
                case LogType.UsuárioKickado:
                        emb.WithTitle(":point_left::runner: Kickado")
                            .WithColor(new Color(255, 246, 0))
                            .WithFooter($"ID do usuário:{usuário.Id}")
                            .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                            .WithCurrentTimestamp();
                    break;
                case LogType.UsuárioMutado:
                        emb.WithTitle(":speak_no_evil: Mutado")
                            .WithColor(new Color(103, 106, 112))
                            .WithFooter($"ID do usuário:{usuário.Id}")
                            .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                            .WithCurrentTimestamp();
                    break;
                case LogType.UsuárioDesmutado:
                        emb.WithTitle(":monkey_face: Desmutado")
                            .WithColor(new Color(237, 237, 237))
                            .WithFooter($"ID do usuário:{usuário.Id}")
                            .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                            .WithCurrentTimestamp();
                    break;
                case LogType.Warn:
                        emb.WithTitle(":warning: Warnado")
                            .WithColor(new Color(244, 166, 65))
                            .WithFooter($"ID do usuário:{usuário.Id}")
                            .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                            .WithCurrentTimestamp();
                    break;
                case LogType.TirarWarn:
                        emb.WithTitle(":recycle: Warns Removidos")
                            .WithColor(new Color(255, 234, 173))
                            .WithFooter($"ID do usuário:{usuário.Id}")
                            .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                            .WithCurrentTimestamp();
                    break;
                case LogType.ChatLock:
                    emb.WithTitle(":x: Canal Mutado")
                        .WithDescription(motivo)
                        .WithColor(new Color(244, 65, 119))
                        .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                        .WithCurrentTimestamp();
                    break;
                case LogType.ChatUnlock:
                    emb.WithTitle(":white_check_mark: Canal Desmutado")
                        .WithDescription(motivo)
                        .WithColor(new Color(92, 244, 66))
                        .WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                        .WithCurrentTimestamp();
                    break;
            }

            await canalLog.SendMessageAsync("", false, emb);
        }

        internal static async Task ChatLogEventosMensagens(LogTypeEventosMensagens logType, SocketTextChannel canalLog,
            SocketUser usuário, SocketTextChannel canalDoEvento, string mensagem = null, string urlFile = null, string mensagemNova = null)
        {
            var emb = new EmbedBuilder();

            if (mensagem == null)
            {
                mensagem = ".";
            }

            switch (logType)
            {
                case LogTypeEventosMensagens.MsgDeletada:
                    emb.WithTitle(":fire: Mensagem Deletada")
                        .WithColor(new Color(244, 66, 101));
                    emb.AddField("Deletado por:", usuário.Mention, true);
                    emb.AddField("Canal:", canalDoEvento.Mention, true);
                    //  Verifica se exitem arquivos na mensagem passada
                    emb.AddField("Mensagem:", $"```{mensagem}```");
                    if (urlFile != null) emb.WithDescription($"Essa mensagem continha um [arquivo]({urlFile})");
                    emb.WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                        .WithCurrentTimestamp()
                        .WithFooter($"ID do usuário: {usuário.Id}");
                    break;

                case LogTypeEventosMensagens.MsgEditada:
                    emb.WithTitle(":pencil: Mensagem Editada")
                        .WithColor(new Color(65, 244, 137));
                    emb.AddField("Editado por:", usuário.Mention, true);
                    emb.AddField("Canal:", canalDoEvento.Mention, true);
                    emb.AddField("Antiga mensagem:", $"```{mensagem}```");
                    emb.AddField("Nova mensagem:", $"```{mensagemNova}```");
                    if (urlFile != null) emb.WithDescription($"Essa mensagem continha um [arquivo]({urlFile})");
                    emb.WithAuthor($"{usuário.Username}#{usuário.Discriminator}", usuário.GetAvatarUrl())
                        .WithCurrentTimestamp()
                        .WithFooter($"ID do usuário: {usuário.Id}");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }

            await canalLog.SendMessageAsync("", false, emb.Build());
        }

        private static ConsoleColor SeverityConsoleColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.DarkRed;
                case LogSeverity.Debug:
                    return ConsoleColor.DarkCyan;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Info:
                    return ConsoleColor.Cyan;
                case LogSeverity.Verbose:
                    return ConsoleColor.Green;
                case LogSeverity.Warning:
                    return ConsoleColor.Magenta;
                default:
                    return ConsoleColor.White;
            }
        }

        internal enum LogTypeEventosMensagens
        {
            MsgDeletada,
            MsgEditada
        }

        internal enum LogType
        {
            UsuárioEntrou,
            UsuárioSaiu,
            UsuárioBanido,
            UsuárioKickado,
            UsuárioMutado,
            UsuárioDesmutado,
            Warn,
            TirarWarn,
            ChatLock,
            ChatUnlock
        }
    }
}