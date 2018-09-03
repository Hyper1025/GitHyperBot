using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules.Admin.Dependencies
{
    public class LimparService
    {
        internal static async Task LimparMensagensTask(SocketTextChannel canal, uint quantidade)
        {
            //  Obtemos o número de mensagens que precisamos apagar
            var msgs = await canal.GetMessagesAsync((int)quantidade + 1).Flatten();
            //  Desse número, selecionamos as que foram criadas até 14 dias da data atual
            var resultado = msgs.Where(x => x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
            //  Criamos uma variável enumerável, só para contar os itens retornados da variável resultado
            //  E a convertemos em uma lista
            var enumerable = resultado.ToList();
            //  Contamos o número de intens na variável enumerable, que será o número de mensagenss apagadas com sucesso
            var numero = enumerable.ToList().Count;
            //  Deletamos as mensagens
            await canal.DeleteMessagesAsync(enumerable);

            //  Isso só serve para uma resposta mais congruente com o resultado da operação
            //  Caso a quantidade solicitada, for igual ao número de mensagens apagadas
            if (quantidade == numero)
            {
                //  Retornamos uma mensagem de sucesso dizendo que todas as tantas mensangens foram apagadas
                //  Essa mensagem será apagada em 5 segundos
                var msgSucesso = await canal.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Deletadas", $"Certo, deletei todas as {quantidade} mensagens", EmbedMessageType.Success, false));
                await Task.Delay(5000);
                await msgSucesso.DeleteAsync();
            }
            else
            {
                //  Caso a quantidade solicitada, for diferente do número de mensagens apagadas
                //  Retornamos uma outra mensagem para o usuário, explicando o que aconteceu...
                //  Essa mensagem é apagada em 10 segundos, para o usuário ler tranquilamente...
                var msgSemiSucesso = await canal.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Deletadas", $"Deletei **{numero - 1}, das {quantidade}...**\n"
                                                                               , EmbedMessageType.Success, false));
                await Task.Delay(10000);
                await msgSemiSucesso.DeleteAsync();
            }
        }

        internal static async Task LimparMensagensUsuário(SocketTextChannel canal, uint quantidade, SocketGuildUser usuário)
        {
            //  Obtemos o número de mensagens que precisamos apagar
            var msgs = await canal.GetMessagesAsync((int)quantidade + 1).Flatten();
            //  Desse número, selecionamos as que foram criadas até 14 dias da data atual, pertencentes ao usuário que queremos
            var resultado = msgs.Where(x => x.Author.Id == usuário.Id && x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
            //  Criamos uma variável enumerável, só para contar os itens retornados da variável resultado
            //  E a convertemos em uma lista
            var enumerable = resultado.ToList();
            //  Contamos o número de intens na variável enumerable, que será o número de mensagenss apagadas com sucesso
            var numero = enumerable.ToList().Count;
            //  Deletamos as mensagens
            await canal.DeleteMessagesAsync(enumerable);

            //  Isso só serve para uma resposta mais congruente com o resultado da operação
            //  Caso a quantidade solicitada, for igual ao número de mensagens apagadas
            if (quantidade == numero)
            {
                //  Retornamos uma mensagem de sucesso dizendo que todas as tantas mensangens foram apagadas
                //  Essa mensagem será apagada em 5 segundos
                var msgSucesso = await canal.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Deletadas", $"Certo, deletei todas as {quantidade} mensagens de {usuário.Mention}",
                    EmbedMessageType.Success, false));

                await Task.Delay(5000);
                await msgSucesso.DeleteAsync();
            }
            else
            {
                //  Caso a quantidade solicitada, for diferente do número de mensagens apagadas
                //  Retornamos uma outra mensagem para o usuário, explicando o que aconteceu...
                //  Essa mensagem é apagada em 10 segundos, para o usuário ler tranquilamente...
                var msgSemiSucesso = await canal.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Deletadas", $"Deletei **{numero - 1}, das {quantidade} de {usuário.Mention}...**\n",
                    EmbedMessageType.Success, false));

                await Task.Delay(10000);
                await msgSemiSucesso.DeleteAsync();
            }
        }
    }
}