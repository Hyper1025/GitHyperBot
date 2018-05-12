using System;
using System.Threading.Tasks;
using Discord;

namespace GitHyperBot.Core.Services
{
    //  <summary>
    //  Classe responsável por gerar o log que é visível no console
    //  Futuramente também irá enviar as mensagens para o chat de log
    //  de eventos do servidor
    //  </summary>

    internal class Logger
    {
        internal static Task Log(LogMessage logMessage)
        {
            Console.ForegroundColor = SeverityConsoleColor(logMessage.Severity);
            string message = String.Concat(DateTime.Now.ToShortTimeString(), " [", logMessage.Source, "] ", logMessage.Message);
            Console.WriteLine(message);
            Console.ResetColor();
            return Task.CompletedTask;
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
    }
}