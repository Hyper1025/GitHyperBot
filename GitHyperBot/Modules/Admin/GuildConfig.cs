using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Admin.Dependencies;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Admin
{
    public class GuildConfig : ModuleBase<SocketCommandContext>
    {
        [Group("GuildConfig")]
        [Alias("GC", "Configurar")]
        [Summary("Configuração da guild")] //  Criamos um grupo para definir configurações
        [RequireUserPermission(GuildPermission
            .Administrator)] //  Requerimos permissão de adm para usar esse grupo de cmd's
        internal class ConfigGruoup : ModuleBase<SocketCommandContext>
        {
            [Command]
            [CmdCategory(Categoria = CmdCategory.Configurações)]
            internal async Task GuildConfig()
            {
                await ReplyAsync("Para você configurar o servidor serão necessários alguns comandos:", false,
                    EmbedHandler.CriarEmbed(
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
                    EmbedMessageType.Info, false));
            }

            //  Configs ID's Canais
            [Command("ChatGeral")]
            [Summary("Define o chat geral do servidor")]
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
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Canal de texto: \n``{Context.Channel.Name}``\nId: \n``{Context.Channel.Id}``\nFoi definido como **{canal}**",
                    EmbedMessageType.Config, false, Context.User));
            }

            [Command("ChatLog")]
            [Summary("Define o chat de log")]
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

            [Command("ChatConvites")]
            [Summary("Define o chat de log")]
            internal async Task DefinirChatConvitesTask()
            {
                const string canal = "chat de convites";
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                guild.IdChatConvites = Context.Channel.Id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Canal de texto: \n``{Context.Channel.Name}``\nId: \n``{Context.Channel.Id}``\nFoi definido como **{canal}**\n" +
                    "Aqui será permitido o envio de convites do discord",
                    EmbedMessageType.Config, false, Context.User));
            }

            [Command("Convites")]
            [Summary("Define o chat de convites")]
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
            [Command("Warns")]
            [Summary("Define o numero máximo de warns, para levar ban")]
            internal async Task DefinirMaximoWarns(uint valor = 0)
            {
                //  Verificamos se o usuário passou um valor
                //  E se ele é diferente do valor padrão que é 0
                if (valor == 0)
                {
                    //  Caso seja 0
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("", "Defina um valor maior que ``0``",
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
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Agora o usuário tem que levar {valor} warns, para levar ban",
                    EmbedMessageType.Config, false, Context.User));
            }

            //  Boas vindas chat geral
            [Command("BoasVindas")]
            [Summary("Ativa ou desativa a mensagem de boas vindas")]
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

            [Command("BoasVindasTítulo")]
            [Summary("Define o titulo da embed de boas vindas do chat geral")]
            [Alias("BoasVindasTitulo")]
            internal async Task BoasVindasTitleTask([Remainder] string texto = null)
            {
                //  Verificamos se o texto é nulo
                if (texto == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
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

            [Command("BoasVindasDescrição")]
            [Summary("Define a descrição da embed de boas vindas do chat geral")]
            [Alias("BoasVindasDescriçao")]
            internal async Task BoasVindasDescriptionTask([Remainder] string texto = null)
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

            [Command("BoasVindasImg")]
            [Summary("Define a descrição da embed de boas vindas do chat geral")]
            [Alias("BoasVindaslink", "BoasVindasImagem", "BoasVindasUrl")]
            internal async Task BoasVindasUrlTask([Remainder] string texto = null)
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
            [Command("BoasVindasPV")]
            [Summary("Ativa ou desativa mensagem de boas vindas por mensagem direta")]
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

            [Command("BoasVindasPVTítulo")]
            [Summary("Define o titulo da embed de boas vindas do chat geral")]
            internal async Task BoasVindasPvTitleTask([Remainder] string texto = null)
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

            [Command("BoasVindasPvDescrição")]
            [Summary("Define a descrição da embed de boas vindas do chat geral")]
            internal async Task BoasVindasPvDescripionTask([Remainder] string texto = null)
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

            [Command("BoasVindasPvImg")]
            [Summary("Define a imagem da embed de boas vindas do chat geral")]
            internal async Task BoasVindasPvUrlTask([Remainder] string texto = null)
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
            [Summary("Clona as configurações da mensagem de boas vindas, para a mensagem que será enviada no privado")]
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

                //  Verificamos tumb do usuário
                guild.BoasVindasPvTumbUsuario = guild.BoasVindasTumbUsuario;
                clonado += "Status TumbUsuário\n\n";

                //  Salvamos
                GuildsMannanger.SaveGuilds();
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("", $"Segue a lista de itens copiados: ```{clonado}```",
                        EmbedMessageType.Config,
                        false, Context.User));
            }

            [Command("GetRoleID")]
            [Summary("Pega o ID baseada no nome da role")]
            internal async Task PegarIdDaRoleTask([Remainder] string nomeDaRole)
            {
                var roleRetornada = Context.Guild.Roles.FirstOrDefault(x =>
                    string.Equals(x.Name, nomeDaRole, StringComparison.CurrentCultureIgnoreCase));
                if (roleRetornada == null)
                {
                    await ReplyAsync("", false,
                        EmbedHandler.CriarEmbed("Ops...", $"Não encontrei a role ``{nomeDaRole}``",
                            EmbedMessageType.Error));
                }
                else
                {
                    await ReplyAsync("", false,
                        EmbedHandler.CriarEmbed($"Id de {nomeDaRole}", roleRetornada.Id.ToString(),
                            EmbedMessageType.Success));
                }

            }

            [Command("DefinirRoleRegras")]
            [Summary("Define a role que será dada ao usuário concordar com os termos das regras")]
            internal async Task DefinirRoleRegrasTask([Remainder] string cargo)
            {
                var guildAccount = GuildsMannanger.GetGuild(Context.Guild);
                var id = GetRoleIdService.GetRole(Context.Guild, cargo);
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == id);

                if (role == null)
                {
                    await ReplyAsync("", false,
                        EmbedHandler.CriarEmbed("Erro!", "ID Inválido", EmbedMessageType.Error, false));
                    return;
                }

                guildAccount.IdRoleRegras = id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sucesso", $"Role **{role.Name}, ID {id}** definida com êxito",
                        EmbedMessageType.Success, false));
            }

            [Command("DefinirRoleRegistrado")]
            [Summary("Define a role que será dada ao usuário quando o registro for concluido")]
            internal async Task DefinirRoleRegistradoTask([Remainder] string cargo)
            {
                var guildAccount = GuildsMannanger.GetGuild(Context.Guild);
                var id = GetRoleIdService.GetRole(Context.Guild, cargo);
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == id);

                if (role == null)
                {
                    await ReplyAsync("", false,
                        EmbedHandler.CriarEmbed("Erro!", "ID Inválido", EmbedMessageType.Error, false));
                    return;
                }

                guildAccount.IdRoleRegistrado = id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sucesso", $"Role **{role.Name}, ID {id}** definida com êxito",
                        EmbedMessageType.Success, false));
            }

            [Command("DefinirRoleFeminino")]
            [Summary("Define a role feminino, servirá para confirmar o registro")]
            internal async Task DefinirRoleFemininoTask([Remainder] string cargo)
            {
                var guildAccount = GuildsMannanger.GetGuild(Context.Guild);
                var id = GetRoleIdService.GetRole(Context.Guild, cargo);
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == id);

                if (role == null)
                {
                    await ReplyAsync("", false,
                        EmbedHandler.CriarEmbed("Erro!", "ID Inválido", EmbedMessageType.Error, false));
                    return;
                }

                guildAccount.IdRoleFemenino = id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sucesso", $"Role **{role.Name}, ID {id}** definida com êxito",
                        EmbedMessageType.Success, false));
            }

            [Command("DefinirRoleMasculino")]
            [Summary("Define a role masculino, servirá para confirmar o registro")]
            internal async Task DefinirRoleMasculinoTask([Remainder] string cargo)
            {
                var guildAccount = GuildsMannanger.GetGuild(Context.Guild);
                var id = GetRoleIdService.GetRole(Context.Guild, cargo);
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == id);

                if (role == null)
                {
                    await ReplyAsync("", false,
                        EmbedHandler.CriarEmbed("Erro!", "ID Inválido", EmbedMessageType.Error, false));
                    return;
                }

                guildAccount.IdRoleMasculino = id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sucesso", $"Role **{role.Name}, ID {id}** definida com êxito",
                        EmbedMessageType.Success, false));
            }

            [Command("DefinirRoleAutomática")]
            [Summary(
                "Define a role automática, é a que será dada quando o usuário entrar no servidor, e removida ao completar o registro")]
            internal async Task DefinirRoleAutomáticaTask([Remainder] string cargo)
            {
                var guildAccount = GuildsMannanger.GetGuild(Context.Guild);
                var id = GetRoleIdService.GetRole(Context.Guild, cargo);
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == id);

                if (role == null)
                {
                    await ReplyAsync("", false,
                        EmbedHandler.CriarEmbed("Erro!", "ID Inválido", EmbedMessageType.Error, false));
                    return;
                }

                guildAccount.IdRoleSemRegistro = id;
                GuildsMannanger.SaveGuilds();

                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sucesso", $"Role **{role.Name}, ID {id}** definida com êxito",
                        EmbedMessageType.Success, false));
            }

            [Command("TopicoChatGeral")]
            [Summary("Ativa ou desativa o contador no tópico do chat geral")]
            internal async Task TopicoChatGeral()
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
                if (guild.TopicoChatGeralBool)
                {
                    //  Caso no arquivo estiver "True", entra aqui
                    //  Definimos o novo valor como "False"
                    guild.TopicoChatGeralBool = false;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Mudança de tópicos automática no chat geral foi desativada",
                        EmbedMessageType.Config, false, Context.User));
                }
                else
                {
                    //  Caso no arquivo estiver "False", entra aqui
                    //  Definimos o novo valor como "True"
                    guild.TopicoChatGeralBool = true;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Mudança de tópicos automática no chat geral foi ativada.\n" +
                        $"O chat <#{guild.IdChatGeral}> agora apresentará em seu tópico um contador de membros\n" +
                        "Você pode modificar o texto padrão usando o seguinte comando:\n" +
                        $"``{Config.Bot.CmdPrefix}Gc DefinirTopico <tópico>``\n" +
                        "Não se esqueça de usar ``{contador}`` no texto para exibi-lo.",
                        EmbedMessageType.Config, false, Context.User));
                }

                //  Salvamos o arquivo
                GuildsMannanger.SaveGuilds();
            }

            [Command("DefinirTopico")]
            [Summary("Define o texto que será exibido no tópico do chat geral")]
            internal async Task DefinirTopicoTask([Remainder] string tópico)
            {
                if (tópico == null)
                {
                    //  Caso seja respondemos o usuário de forma negativa
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa, você você não me disse o tópico...\n" +
                        $"Você deve fazer assim: ``{Config.Bot.CmdPrefix}Gc DefinirTopico <tópico>``\n" +
                        "Não se esqueça de usar ``{contador}`` onde quiser exibi-lo."
                        , EmbedMessageType.Info, false, Context.User));
                    return; // Retornamos do método
                }

                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);

                //  Verificamos se o chat geral já foi definido
                if (guild.IdChatGeral == 0)
                {
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa... Parece que você ainda não tinha definido o chat geral, irei definir esse chat como chat geral para você...\n" +
                        $"Não se preocupe, você pode alterar o chat usando o comando ``{Config.Bot.CmdPrefix}Gc ChatGeral``",
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

                //  Caso não teja ativo, o bot avisa
                if (guild.TopicoChatGeralBool == false)
                {
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        $"Não se esqueça de ativar a mudança de tópico automática, com... ``{Config.Bot.CmdPrefix}Gc DefinirTopico``",
                        EmbedMessageType.Config, false, Context.User));
                }

                //  Definimos o tópico
                guild.TopicoChatGeral = tópico;

                //  Salvamos
                GuildsMannanger.SaveGuilds();

                //  Respondemos o usuário de forma positiva
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                    $"Muito bem... o tópico do chat ``<#{guild.IdChatGeral}>`` foi definido como ``{tópico}``"
                    , EmbedMessageType.Config, false, Context.User));
            }

            [Command("DelayAutoRole")]
            [Summary("Ativa ou desativa o delay para dar a role automática")]
            internal async Task DelayAutoRoleTask()
            {
                //  Obtemos a guild
                var guild = GuildsMannanger.GetGuild(Context.Guild);
                //  Verificamos se a role já foi definida
                if (guild.IdRoleSemRegistro == 0)
                {
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Opa... Parece que a role automática ainda não foi definida...\n" +
                        $"Não se preocupe, você pode alterar o chat usando o comando ``{Config.Bot.CmdPrefix}DefinirRoleAutomática <role>``",
                        EmbedMessageType.Error, false, Context.User));
                    return;
                }

                //  Verificamos o valor que já estava no arquivo (por padrão é falso)
                if (guild.DelayAutoRole)
                {
                    //  Caso no arquivo estiver "True", entra aqui
                    //  Definimos o novo valor como "False"
                    guild.DelayAutoRole = false;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Delay de auto-role desativado",
                        EmbedMessageType.Config, false, Context.User));
                }
                else
                {
                    //  Caso no arquivo estiver "False", entra aqui
                    //  Definimos o novo valor como "True"
                    guild.DelayAutoRole = true;
                    //  Enviamos uma resposta ao usuário
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("",
                        "Delay de auto-role ativado",
                        EmbedMessageType.Config, false, Context.User));
                }

                //  Salvamos o arquivo
                GuildsMannanger.SaveGuilds();
            }
        }
    }
}