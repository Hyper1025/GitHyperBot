using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Core.Services;

namespace GitHyperBot
{
    //  <summary>
    //  Classe Principal 
    //  </summary>

    internal class Program
    {
        //  Discord client e command service
        private DiscordSocketClient _client;
        private CommandHandler _cmdHandler;

        internal static void Main()
        {
            Console.Title = "HyperBot - v1.0";
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
                Console.ReadKey();
                return;     
            }

            Console.WriteLine("Token encontrado, iniciando conexão.\n");
            Console.ResetColor();
            Console.Clear();

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

            //  Eventos
            _client.Log += Logger.ConsoleLog;
            _client.UserJoined += EventosUsuário.NovoMembro;
            _client.UserBanned += EventosUsuário.UsuárioBanido;
            _client.UserLeft += EventosUsuário.UsuárioSaiu;
            _client.UserUnbanned += EventosUsuário.UsuárioDesbanido;
            _client.ReactionAdded += ReactionsHandler.ReaçãoAdicionada;
            _client.MessageReceived += RecompensasService.HandleMsgRewards;
            _client.MessageDeleted += MsgDeletada.MensagemDeletada;
            _client.MessageUpdated += MsgEditada.MensagemEditada;
            _client.Ready += ReadyServices.StartTimer;

            //  Iniciamos a conexão com o discord
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();

            //  Isso servirá para nos acessarmos o client de qualquer classe
            //  Quando necessário
            Global.Client = _client;

            _cmdHandler = new CommandHandler();
            await _cmdHandler.InitializeAsync(_client);

            //  Previne do programa fechar
            await Task.Delay(-1);
        }
    }
}
