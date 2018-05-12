using System;
using System.Threading.Tasks;
using Discord;
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
            Console.Title = "HyperBot v1.0";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Inicializando HyperBot...");

            //  Verificamos se o token existe
            if (Config.Bot.Token == null)
            {
                //  Caso não exista
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Por favor preencha o token no arquivo de configuração.");
                Console.WriteLine("Esse arquivo se chama 'Data.json', e está presente na pasta 'Resources'\n");
                Console.ResetColor();
                return;     
            }

            Console.WriteLine($"Token: {Config.Bot.Token}\n");
            Console.ResetColor();

            //  Iniciamos a classe "StartAsync" de forma assíncrona
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        public async Task StartAsync()
        {
            //  Criamos um cliente
            //  E atribuimos as suas devidas configurações
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                //  Loglevel é o nível minimo para ser gerado um log
                LogLevel = LogSeverity.Verbose,

                //  Tamanho máximo das mensagens a serem processadas pelo bot
                //  Mais de 500 caracteres é ignorado
                MessageCacheSize = 500
            });

            _client.Log += Core.Services.Logger.Log;
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();

            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);

            //  Previne do programa fechar
            await Task.Delay(-1);
        }
    }
}
