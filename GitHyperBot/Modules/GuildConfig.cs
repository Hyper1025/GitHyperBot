using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules
{
    public class GuildConfig : ModuleBase<SocketCommandContext>
    {
        [Group("GuildConfig"), Alias("GC","Configurar"), Summary("Configuração da guild")]  //  Criamos um grupo para definir configurações
        [RequireUserPermission(GuildPermission.Administrator)]                              //  Requerimos permissão de adm para usar esse grupo de cmd's
        internal class ConfigGruoup : ModuleBase<SocketCommandContext>
        {
            [Command]
            internal async Task GuildConfig()
            {
                await ReplyAsync("Para você configurar o servidor serão necessários alguns comandos:", false, EmbedHandler.CriarEmbed(
                    "Configurando Chats:",
                    "Eu preciso classificar os chats em **chat geral, log e convites.**\n" +

                    "Para definir o **chat geral** use o comando a baixo no chat geral do seu servidor:" +
                    $"```{Config.Bot.CmdPrefix}Gc ChatGeral```" +

                    "Para definir o **chat de log** use o comando a baixo no chat de log do seu servidor:" +
                    $"```{Config.Bot.CmdPrefix}Gc ChatLog```" +

                    "Para definir o **chat de convites** use o comando a baixo no chat de convites do seu servidor:" +
                    $"```{Config.Bot.CmdPrefix}Gc ChatConvites```",
                    EmbedMessageType.Info, false, Context.User));

                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Configurando Warns:",
                    "Não é preciso, porém você pode mudar o número de warns necessários para eu dar ban em alguém.\n" +
                    "**O valor padrão é de 4 warns**, caso queira mudar esse valor, use:" +
                    $"```{Config.Bot.CmdPrefix}Gc Warns Digite aqui o númuero```",
                    EmbedMessageType.Warning, false));

                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Configurando Boas vindas do chat geral:",
                    "Essa será a mensagem que será enviada no chat geral quando alguém entrar no seu servidor.\n" +
                    "Precisamos configurar alguns parâmetros dela. Que são eles **Título, Descrição, Imagem.**\n" +

                    "Título da mensagem de boas vindas no chat geral:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindasTítulo Digite aqui o título```" +
                    
                    "Descrição da mensagem de boas vindas no chat geral:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindasDescrição Digite aqui a descrição```" +
                    "Na descrição você pose usar **{mencionar}** para mencionar o usuário. E **contra barra seguida da letra n** para pular de linha.\n" +
                    
                    "Imagem da mensagem de boas vindas no chat geral:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindasImg Insira o link aqui```" +
                    
                    "Para habilitar e desabilitar o envio das boas vindas no seu chat geral, use:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindas```",
                    EmbedMessageType.Info, false));

                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Configurando Boas vindas por mensagem privada",
                    "Você pode simplesmente clonar a mensagem que você envia no chat geral para o privado, usando o comando:" +
                    $"```{Config.Bot.CmdPrefix}Gc ClonarBoasVindasParaPv```" +
                    "E editar o que achar que for necessário...\n" +

                    "Título da mensagem de boas vindas privada:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindasPVTítulo Digite aqui o título```" +

                    "Descrição da mensagem de boas vindas privada:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindasPvDescrição Digite aqui a descrição```" +
                    "Na descrição você pose usar **{mencionar}** para mencionar o usuário. E **contra barra seguida da letra n** para pular de linha.\n" +
                    
                    "Imagem da mensagem de boas vindas privada:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindasPvImg Insira o link aqui```" +
                    
                    "Para habilitar e desabilitar o envio das boas vindas no privado, use:" +
                    $"```{Config.Bot.CmdPrefix}Gc BoasVindasPV```", 
                    EmbedMessageType.Info,false));
            }

            //  Configs ID's Canais
            [Command("ChatGeral"), Remarks("Define o chat geral do servidor")]
            public async Task DefinirChatGeralTask()
            {
                
                //  Apenas uma string para facilitar as coisas
                const string canal = "chat geral";
                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                //  Definimos o valor da propriedade
                guild.IdChatGeral = Context.Channel.Id;
                //  Salvamos
                GuildsMannanger.SaveGuilds();

                //  Enviamos uma resposta para o usuário
                await ReplyAsync("",false,EmbedHandler.CriarEmbed("",
                    $"Canal de texto: \n``{Context.Channel.Name}``\nId: \n``{Context.Channel.Id}``\nFoi definido como **{canal}**", 
                    EmbedMessageType.Config, false, Context.User));
            }

            [Command("ChatLog"), Remarks("Define o chat de log")]
            internal async Task DefinirChatLogTask()
            {
                //  Exatamente o mesmo do comando anterior...
                const string canal = "chat de log";
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                guild.IdChatLog = Context.Channel.Id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false, EmbedHandler.CriarEmbed("", 
                    $"Canal de texto: \n``{Context.Channel.Name}``\nId: \n``{Context.Channel.Id}``\nFoi definido como **{canal}**",
                    EmbedMessageType.Config, false, Context.User));
            }

            [Command("ChatConvites"), Remarks("Define o chat de log")]
            internal async Task DefinirChatConvitesTask()
            {
                const string canal = "chat de convites";
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                guild.IdChatConvites = Context.Channel.Id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Canal de texto: \n``{Context.Channel.Name}``\nId: \n``{Context.Channel.Id}``\nFoi definido como **{canal}**\n" +
                    $"Aqui será permitido o envio de convites do discord",
                    EmbedMessageType.Config, false, Context.User));
            }

            [Command("Convites"), Remarks("Define o chat de convites")]
            internal async Task BloquearConvitesTask()
            {
                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                //  Verificamos o valor que já estava no arquivo (por padrão é falso)
                if (guild.BlockInvites)
                {
                    //  Caso no arquivo estiver True, entra aqui
                    //  E então iremos definir como falso
                    guild.BlockInvites = false;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Agora convites serão deletados, e quem enviou, levará warn...\n" +
                        "Isso não se aplica a membros da staff",
                        EmbedMessageType.Config, false, Context.User));
                }
                else
                {
                    //  Caso o valor no arquivo for "false"
                    //  Definimos o valor como "true"
                    guild.BlockInvites = true;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Agora convites serão deletados, e quem enviou, levará warn...\n" +
                        "Isso não se aplica a membros da staff",
                        EmbedMessageType.Config, false, Context.User));
                }

                //  Salvamos o arquivo
                GuildsMannanger.SaveGuilds();
            }

            //  Warns
            [Command("Warns"), Remarks("Define o numero máximo de warns, para levar ban")]
            internal async Task DefinirMaximoWarns(uint valor = 0)
            {
                //  Verificamos se o usuário passou um valor
                //  E se ele é diferente do valor padrão que é 0
                if (valor == 0)
                {
                    //  Caso seja 0
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("", $"Defina um valor maior que ``0``",
                        EmbedMessageType.Error, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                //  Definimos o valor
                guild.MaxWarning = valor;
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                //  Enviamos uma resposta para o usuário
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("", $"Agora o usuário tem que levar {valor} warns, para levar ban",
                    EmbedMessageType.Config, false, Context.User));
            }

            //  Boas vindas chat geral
            [Command("BoasVindas"), Remarks("Ativa ou desativa a mensagem de boas vindas")]
            internal async Task BoolBoasVindasTask()
            {
                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Verificamos se o chat geral já foi definido
                if (guild.IdChatGeral == 0)
                {
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa... Parece que você ainda não tinha definido o chat geral, irei definir esse chat como chat geral para você...\n" +
                        $"Não se preocupe, você pode alterar o chat usando o comando ``{Config.Bot.CmdPrefix}GuildConfig ChatGeral``",
                        EmbedMessageType.Info, false, Context.User));

                    //  Delayzinho básico
                    await Task.Delay(500);
                    //  Definimos o valor da propriedade
                    guild.IdChatGeral = Context.Channel.Id;
                    //  Enviamos uma resposta para o usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        $"Canal de texto: \n``{Context.Channel.Name}``\nId: \n``{Context.Channel.Id}``\nFoi definido como **chat geral**",
                        EmbedMessageType.Config, false, Context.User));

                }

                //  Verificamos o valor que já estava no arquivo (por padrão é falso)
                if (guild.BoasVindasBool)
                {
                    //  Caso no arquivo estiver "True", entra aqui
                    //  Definimos o novo valor como "False"
                    guild.BoasVindasBool = false;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Mensagem de boas vindas no chat geral foi desativada",
                        EmbedMessageType.Config, false, Context.User));
                }
                else
                {
                    //  Caso no arquivo estiver "False", entra aqui
                    //  Definimos o novo valor como "True"
                    guild.BoasVindasBool = true;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Mensagem de boas vindas no chat geral foi ativada.\n" +
                        $"Ela será enviada em <#{guild.IdChatGeral}>\n" +
                        "Não se esqueça de configurar os seus parâmetros, caso ainda não tenha os configurado.\n" +
                        "Como Título, Descrição, Imagem... e assim por diante",
                        EmbedMessageType.Config, false, Context.User));
                }

                //  Salvamos o arquivo
                GuildsMannanger.SaveGuilds();
            }

            [Command("BoasVindasTítulo"), Remarks("Define o titulo da embed de boas vindas do chat geral")]
            [Alias("BoasVindasTitulo")]
            internal async Task BoasVindasTitleTask([Remainder]string texto = null)
            {
                //  Verificamos se o texto é nulo
                if (texto == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("",false,EmbedHandler.CriarEmbed("",
                        "Opa, você você não me disse o **título** que você quer usar na embed de boas vindas...\n" +
                        $"você deve fazer assim: ``{Config.Bot.CmdPrefix}GuildConfig TituloBoasVindas Digite aqui o título que desejar``"
                        , EmbedMessageType.Info, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Definimos o texto
                guild.BoasVindasTitle = texto;
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                //  Respondemos o usuário de forma positiva
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Muito bem... ``{texto}`` foi atribuido ao **título** da mensagem de boas vindas que será enviada no **chat geral**"
                    , EmbedMessageType.Config, false, Context.User));
            }

            [Command("BoasVindasDescrição"), Remarks("Define a descrição da embed de boas vindas do chat geral")]
            [Alias("BoasVindasDescriçao")]
            internal async Task BoasVindasDescriptionTask([Remainder]string texto = null)
            {
                //  Verificamos se o texto é nulo
                if (texto == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa, você você não me disse a **descrição** que você quer usar na embed de boas vindas...\n" +
                        $"Você deve fazer assim: ``{Config.Bot.CmdPrefix}GuildConfig DescriçãoBoasVindas Digite aqui a descrição que desejar``"
                        , EmbedMessageType.Info, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Definimos o texto
                guild.BoasVindasDescription = texto;
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                //  Respondemos o usuário de forma positiva
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Muito bem... ``{texto}`` foi atribuido a **descrição** da mensagem de boas vindas que será enviada no **chat geral**"
                    , EmbedMessageType.Config, false, Context.User));
            }

            [Command("BoasVindasImg"), Remarks("Define a descrição da embed de boas vindas do chat geral")]
            [Alias("BoasVindaslink", "BoasVindasImagem", "BoasVindasUrl")]
            internal async Task BoasVindasUrlTask([Remainder]string texto = null)
            {
                //  Verificamos se o texto é nulo
                if (texto == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa, você você não me disse a **URL** da imagem que você quer usar na embed de boas vindas...\n" +
                        $"Você deve fazer assim: ``{Config.Bot.CmdPrefix}GuildConfig BoasVindasUrl Insira o link da imagem aqui"
                        , EmbedMessageType.Info, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Definimos o texto
                guild.BoasVindasUrl = texto;
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                //  Respondemos o usuário de forma positiva
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Muito bem... ``{texto}`` foi atribuido a **URL** da imagem da mensagem de boas vindas que será enviada no **chat geral**"
                    , EmbedMessageType.Config, false, Context.User));
            }

            //  Boas vindas mensagem direta
            [Command("BoasVindasPV"), Remarks("Ativa ou desativa mensagem de boas vindas por mensagem direta")]
            internal async Task BoolBoasVindasPvTask()
            {
                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                //  Verificamos o valor que já estava no arquivo (por padrão é falso)
                if (guild.BoasVindasPvBool)
                {
                    //  Caso no arquivo estiver "True", entra aqui
                    //  Definimos o novo valor como "False"
                    guild.BoasVindasPvBool = false;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Mensagem de boas vindas no por mensagem direta foi desativada",
                        EmbedMessageType.Config, false, Context.User));
                }
                else
                {
                    //  Caso no arquivo estiver "False", entra aqui
                    //  Definimos o novo valor como "True"
                    guild.BoasVindasPvBool = true;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Mensagem de boas vindas no por mensagem direta foi ativada.\n",
                        EmbedMessageType.Config, false, Context.User));
                }

                //  Salvamos o arquivo
                GuildsMannanger.SaveGuilds();
            }

            [Command("BoasVindasPVTítulo"), Remarks("Define o titulo da embed de boas vindas do chat geral")]
            internal async Task BoasVindasPvTitleTask([Remainder]string texto = null)
            {
                //  Verificamos se o texto é nulo
                if (texto == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa, você você não me disse o **título** que você quer usar na embed **privada** de boas vindas...\n" +
                        $"Você deve fazer assim: ``{Config.Bot.CmdPrefix}GuildConfig BoasVindasPVTítulo Digite aqui o título que desejar``"
                        , EmbedMessageType.Info, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Definimos o texto
                guild.BoasVindasPvTitle = texto;
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                //  Respondemos o usuário de forma positiva
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Muito bem... ``{texto}`` foi atribuido ao **título** da mensagem de boas vindas que será enviada no **privado**"
                    , EmbedMessageType.Config, false, Context.User));
            }

            [Command("BoasVindasPvDescrição"), Remarks("Define a descrição da embed de boas vindas do chat geral")]
            internal async Task BoasVindasPvDescripionTask([Remainder]string texto = null)
            {
                //  Verificamos se o texto é nulo
                if (texto == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa, você você não me disse a **descrição** que você quer usar na embed **privada** de boas vindas...\n" +
                        $"Você deve fazer assim: ``{Config.Bot.CmdPrefix}GuildConfig DescriçãoBoasVindas Digite aqui a descrição que desejar``"
                        , EmbedMessageType.Info, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Definimos o texto
                guild.BoasVindasPvDescription = texto;
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                //  Respondemos o usuário de forma positiva
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Muito bem... ``{texto}`` foi atribuido a **descrição** da mensagem de boas vindas que será enviada no **privado**"
                    , EmbedMessageType.Config, false, Context.User));
            }

            [Command("BoasVindasPvImg"), Remarks("Define a descrição da embed de boas vindas do chat geral")]
            internal async Task BoasVindasPvUrlTask([Remainder]string texto = null)
            {
                //  Verificamos se o texto é nulo
                if (texto == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa, você você não me disse a **URL** da imagem que você quer usar na embed **privada** de boas vindas...\n" +
                        $"Você deve fazer assim: ``{Config.Bot.CmdPrefix}GuildConfig BoasVindasPvImg Insira o link da imagem aqui"
                        , EmbedMessageType.Info, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Definimos o texto
                guild.BoasVindasPvUrl = texto;
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                //  Respondemos o usuário de forma positiva
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Muito bem... ``{texto}`` foi atribuido a **URL** da imagem da mensagem de boas vindas que será enviada no **privado**"
                    , EmbedMessageType.Config, false, Context.User));
            }

            [Command("ClonarBoasVindasParaPv")]
            internal async Task ClonarBoasVindasParaPvTask()
            {
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                var clonado = "";

                //  Verificamos se o título existe
                if (guild.BoasVindasTitle != null)
                {
                    guild.BoasVindasPvTitle = guild.BoasVindasTitle;
                    clonado += $"Título: {guild.BoasVindasPvTitle}\n\n";
                }

                //  Verificamos se a descrição existe
                if (guild.BoasVindasDescription != null)
                {
                    guild.BoasVindasPvDescription = guild.BoasVindasDescription;
                    clonado += $"Descrição: {guild.BoasVindasPvDescription}\n\n";
                }

                //  Verificamos se a url existe
                if (guild.BoasVindasUrl != null)
                {
                    guild.BoasVindasPvUrl = guild.BoasVindasUrl;
                    clonado += $"Url da imagem: {guild.BoasVindasPvUrl}\n\n";
                }
                //  Salvamos
                GuildsMannanger.SaveGuilds();
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("", $"Segue a lista de itens copiados: ```{clonado}```", 
                        EmbedMessageType.Config,
                        false, Context.User));
            }
        }
    }
}