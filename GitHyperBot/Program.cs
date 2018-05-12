using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.WebSocket;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;
        private static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Inicializando HyperBot...");
            if (Config.Bot.Token == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Por favor preencha o token no arquivo de configuração.");
                Console.WriteLine("Esse arquivo se chama 'Data.json', e está presente na pasta 'Resources'\n");
                Console.ResetColor();
                return;     
            }
            Console.WriteLine($"Token: {Config.Bot.Token}");
            Console.ResetColor();

            //  Iniciamos a classe "StartAsync" de forma assíncrona
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        public async Task StartAsync()
        {

        }
    }
}
