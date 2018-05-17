using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace GitHyperBot.Core.Handlers
{
    //  <summary>
    //  Essa classe será a responsável por pegar
    //  os comandos feitos pelos usuários
    //  </summary>

    public class CommandHandler
    {
        //  Atribuimos _client como um DiscordSocketClient
        private DiscordSocketClient _client;
        //  Atribuimos _service como um CommandService
        private CommandService _service;

        //  Classe de inicialização
        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;

            //  Criamos um command service para os comandos
            _service = new CommandService(new CommandServiceConfig  //  Sobrecarregamos o método com "CommandServiceConfig"
            {                                                       //  Isso nos permitirá configurar o Command Service

                CaseSensitiveCommands = false,  //  Os comandos não devem fazer distinção entre maiúsculas e minúsculas 
                DefaultRunMode = RunMode.Async  //  Executamos os comandos de forma assíncrona 
            });

            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            //  Lançamos uma SocketMessage para um SocketUserMessage
            //  Verificamos se a mensagem não é nula
            if (!(s is SocketUserMessage msg)) return;

            //  Agora criamos uma váriavel chamada "context"
            //  Que basicamente é um pacote de informações
            //  Contendo várias coisas que nós iremos precisar nos nossos comandos
            var context = new SocketCommandContext(_client,msg);
            
            //  Definimos uma váriavel que servirá como
            //  Indicador da posição do prefixo do bot
            //  Para que uma mensagem seja considerada um comando
            int argPos = 0;

            //  Verificamos as condições de prefixo
            if (msg.HasStringPrefix(Config.Config.Bot.CmdPrefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser,ref argPos))
            {
                //  Se forem válidas prosseguimos

                //  O trecho "await _service.ExecuteAsync(context, argPos);"
                //  Básicamente executa os comandos
                //  Atribuimos a váriavel "result", para podermos nos referenciar a eventos dele
                var result = await _service.ExecuteAsync(context, argPos);

                //  Verificamos se ocorreu algum erro no comando
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    // <AINDA POR IMPLEMENTAR>
                    await context.Channel.SendFileAsync(result.ErrorReason);
                }
            }
        }
    }
}