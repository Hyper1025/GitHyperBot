using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace GitHyperBot.Core.Handlers
{
    //  Essa classe será a responsável por pegar
    //  os comandos feitos pelos usuários
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
            _service = new CommandService();

            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            //  Lançamos uma SocketMessage para um SocketUserMessage
            var msg = s as SocketUserMessage;
            
            //  Verificamos se a mensagem não é nula
            if (msg == null) return;

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