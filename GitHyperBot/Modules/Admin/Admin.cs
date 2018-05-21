using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules.Admin
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Command("Limpar"), Remarks("Purges An Amount Of Messages")]
        [RequireUserPermission(GuildPermission.ManageMessages), RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task LimparChaTask(uint quantidade = 0)
        {
            //  Verificamos se foi informado um número de mensagens a serem deletadas
            if (quantidade == 0)
            {
                //  Caso o valor seja igual a 0
                var msgErro = await Context.Channel.SendMessageAsync("",false,EmbedHandler.CriarEmbed("", 
                    "Você precisa mensurar um número maior que _0_, para serem deletadas",EmbedMessageType.Info,false,Context.User));
                await Task.Delay(5000);
                await msgErro.DeleteAsync();
                return; //  Retornamos
            }

            //  Capturamos o número de mensagens que precisamos apagar
            var msgs = await Context.Channel.GetMessagesAsync((int)quantidade + 1).Flatten();
            //  Desse número, selecionamos as que foram criadas até 14 dias da data atual
            var resultado = msgs.Where(x => x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
            //  Criamos uma variável enumerável, só para contar os itens retornados da variável resultado
            //  E a convertemos em uma lista
            var enumerable = resultado.ToList();
            //  Contamos o número de intens na variável enumerable, que será o número de mensagenss apagadas com sucesso
            var numero = enumerable.ToList().Count;
            //  Deletamos as mensagens
            await Context.Channel.DeleteMessagesAsync(enumerable);

            //  Isso só serve para uma resposta mais congruente com o resultado da operação
            //  Caso a quantidade solicitada, for igual ao número de mensagens apagadas
            if (quantidade == numero)
            {
                //  Retornamos uma mensagem de sucesso dizendo que todas as tantas mensangens foram apagadas
                //  Essa mensagem será apagada em 5 segundos
                var msgSucesso = await ReplyAsync("", false, EmbedHandler.CriarEmbed("Deletadas", $"Certo, deletei todas as {quantidade} mensagens", EmbedMessageType.Success, false, Context.User));
                await Task.Delay(5000);
                await msgSucesso.DeleteAsync();
            }
            else
            {
                //  Caso a quantidade solicitada, for diferente do número de mensagens apagadas
                //  Retornamos uma outra mensagem para o usuário, explicando o que aconteceu...
                //  Essa mensagem é apagada em 10 segundos, para o usuário ler tranquilamente...
                var msgSemiSucesso = await ReplyAsync("", false, EmbedHandler.CriarEmbed("Deletadas", $"Deletei **{numero - 1}, das {quantidade}...**\n" +
                                                                               $"**{quantidade - numero + 1} não foram deletadas**, devido a terem sido criadas a mais de 14 dias atrás.", EmbedMessageType.Success, false, Context.User));
                await Task.Delay(10000);
                await msgSemiSucesso.DeleteAsync();
            }
        }

    }
}