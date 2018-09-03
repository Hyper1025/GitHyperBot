using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules.Help.Dependencies
{
    public class HelpService
    {
        public static async Task HelpListTask(SocketTextChannel canal,SocketGuildUser usuário)
        {
            var eb = new EmbedBuilder();
            eb.WithAuthor($"Manual de comandos do {Global.Client.CurrentUser.Username}", Global.Client.CurrentUser.GetAvatarUrl())
                .WithDescription($"Use ``{Config.Bot.CmdPrefix}help <comando>``, para obter mais informações sobre o comando\n ``<>`` indica um parâmetro obrigatório.\n``()`` indica um parâmetro opcional")
                .WithColor(Color.Blue);

            var contadorCmds = 0;

            foreach (var auxCmdValue in Enum.GetValues(typeof(CmdCategory)))
            {
                var commandList = (from c in Global.CommandService.Commands
                                   let attribute = (CmdCategoryAttribute)c.Attributes.FirstOrDefault
                                  (x => x is CmdCategoryAttribute)
                                   where attribute?.Categoria == (CmdCategory)auxCmdValue
                                   select c.Name).ToList();

                if ((CmdCategory)auxCmdValue == CmdCategory.Administração && !usuário.GuildPermissions.Administrator)
                {
                    continue;
                }

                if ((CmdCategory)auxCmdValue == CmdCategory.Moderação && !usuário.GuildPermissions.ManageChannels)
                {
                    continue;
                }

                //  Não queremos passar os comandos de config no help, então ao chegar nele
                //  Continuamos o loop
                if ((CmdCategory)auxCmdValue == CmdCategory.Configurações)
                {
                    continue;
                }

                if (!commandList.Any()) continue;
                commandList.Sort();
                var commandString = string.Join(", ", commandList);

                eb.AddField(auxCmdValue.ToString(), commandString);
                contadorCmds += commandList.Count;
            }

            eb.WithFooter($"{contadorCmds} comandos carregados");

            try
            {
                await usuário.SendMessageAsync("", false, eb.Build());
                var msg = await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Enviado", "Enviei a lista de comandos no seu privado", EmbedMessageType.Info,
                        false, usuário));
                await Task.Delay(10000);
                await msg.DeleteAsync();
            }
            catch (Exception e)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Erro",
                        $"Não consegui enviar minha lista de comandos no seu privado, erro:```{e}```\nEnviarei no chat...",
                        EmbedMessageType.Error, false, usuário));
                await canal.SendMessageAsync("", false, eb.Build());
            }
        }

        public static async Task HelpCommandTask(SocketCommandContext context, string command)
        {
            //  Usamos o CommandService para pegar os atributos do comando que se iguala ao comando solicitado
            //  StringComparer executa comparações de cadeia de caracteres que não diferenciam maiúsculas de minúsculas, usando as regras de comparação de palavras da cultura atual
            var cmdInfo = Global.CommandService.Commands.FirstOrDefault(x => x.Aliases.Contains(command, StringComparer.CurrentCultureIgnoreCase));
            
            //  Verificamos se o cmdInfo retornou algo
            if (cmdInfo == null)
            {
                //  Caso não tenha retornado, o comando não existe
                await context.Channel.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Erro","Comando desconhecido",EmbedMessageType.Error,false));
                return;
            }

            //  Criamos uma embed
            var helpBuilder = new EmbedBuilder();
            //  Damos a ela um título e uma cor
            helpBuilder.WithTitle($":information_source: {cmdInfo.Name}").WithColor(Color.Blue);

            //  Verificamos se o comando apresenta um sumário
            if (cmdInfo.Summary != null) helpBuilder.WithDescription($"{cmdInfo.Summary}");
            
            //  Adicionamos um campo para o uso
            //helpBuilder.AddField("Uso:", GetCmdUsage(cmdInfo));
            helpBuilder.AddField("Uso:", GetCmdUsage(cmdInfo));

            //  Se o comando requer parâmetros, explicamos eles no footer da embed  
            if (cmdInfo.Parameters.Count != 0) helpBuilder.WithFooter("Glossário: <requer>, (opcional)", Global.Client.CurrentUser.GetAvatarUrl());       
            
            //  Pegamos a cetegoria do comando
            var categoryAttributes = cmdInfo.Attributes.OfType<CmdCategoryAttribute>();
            //  Se o comando pertencer a uma categoria, adicionamos ela
            var cmdCategoryAttributes = categoryAttributes.ToList();
            if (cmdCategoryAttributes.Any()) helpBuilder.AddField("Categoria:", cmdCategoryAttributes.First().Categoria);
            //  Verificamos a existência de pseudônimos do comando
            if (cmdInfo.Aliases.Any())
            {
                //  Adicionamos eles a um campo chamado "Pseudônimos"
                var aliases = string.Join(", ", cmdInfo.Aliases);
                helpBuilder.AddField("Pseudônimos:", aliases);
            }
            //  Enviamos no chat
            await context.Channel.SendMessageAsync("", false, helpBuilder);
        }

        public static string GetCmdUsage(CommandInfo commandInfo)
        {
            // Criamos as strings string
            var parametros = string.Empty;
            //  Varremos os parâmetros do comando
            foreach (var p in commandInfo.Parameters)
                if (!p.IsOptional)
                {
                    //  Parâmetro requerido
                    parametros += "<" + p.Name + "> ";
                }
                else
                {
                    //  Parâmetro opicional
                    parametros += "(" + p.Name + ") ";
                }

            //  Verificamos se o comando pertence a algum grupo de comando
            //  Jogamos tudo para a string uso
                var uso = Config.Bot.CmdPrefix + commandInfo.Name;
            //  Se parâmetros for diferente de nulo, adicionamos eles
            //  O uso do operador += nesse contexto é conhecido como inscrever-se em um evento
            //  ou seja operador += acrescenta algo a uma string existênte
            if (parametros != string.Empty) uso += $" {parametros}";
            //  Retormamos a string
            return uso;
        }

        public static CommandInfo GetCommandInfo(SocketCommandContext context, int argPos = 0)
        {
            return Global.CommandService.Commands.FirstOrDefault(x =>
                x.Aliases.Contains(Global.CommandService.Search(context, argPos).Commands.FirstOrDefault().Alias));
        }
    }
}