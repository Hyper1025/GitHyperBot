using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Features.Economia;

namespace GitHyperBot.Modules
{
    //  <Sumary>
    //  Comandos relacionados ao sistema de economia
    //  </Sumary>

    public class Economia : ModuleBase<SocketCommandContext>
    {
        [Command("Daily"), Remarks("Dá uma quantidade de gold por dia.")]
        [Alias("GetDaily", "ClaimDaily", "GetGold", "ClaimGold")]
        public async Task GetDailyTask()
        {
            //  Chama o método "GetDailyResult" passando o usuário que executou o comando
            //  Atribui o retorno do método a variável resultado
            var resultado = Dayli.GetDailyResult(Context.User);

            //  Verifica o resultado
            if (resultado.Sucesso)
            {
                //  Caso sucesso for verdadeiro
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("", $"Você ganhou {Constantes.DailyGold} golds",
                        EmbedMessageType.GoldGain, false, Context.User));
            }
            else
            {
                //  Caso sucesso for falso
                var tempo = string.Format("{0:%h} horas {0:%m} minutos {0:%s} segundos", resultado.IntervaloDeTempo);

                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("",
                        $"Você já pegou o seu **Gold** diário.\nVolte novamente em **{tempo}**",
                        EmbedMessageType.Info, false, Context.User));
            }
        }

        [Command("Golds"), Remarks("Mostra quantos golds você tem.")]
        [Alias("Gold")]
        public async Task ChecarGoldsTask()
        {
            var account = AccountsMananger.GetAccount(Context.User);
            await ReplyAsync("", false,
                EmbedHandler.CriarEmbed("", $"Você tem **{account.Gold} golds**", EmbedMessageType.GoldGain, false, Context.User));
        }

        [Command("Transferir"), Remarks("Transfere dinheiro para outra pessoa")]
        public async Task TransferirGoldTask(IGuildUser para, ulong montante = 0)
        {
            //  Chamamos o método de "DeUserParaUseResult", presente na classe Transfer
            //  Passando "Transfer.DeUserParaUseResult(De quem, para quem, montante)"
            var resultado = Transfer.DeUserParaUseResult(Context.User, para, montante);

            //  Verificamos o resultado
            //  Em determinados casos "switch" é uma melhor alternativa
            //  do que usar "if", já que ele (swith) entre direto na "resposta"

            //  "O switch vai direto a respota, e não tem que verificar todas alternativas"
            switch (resultado)
            {
                case Transfer.Result.SelfTransfer:
                    await ReplyAsync("",false, EmbedHandler.CriarEmbed("", "Você não pode transferir para sí próprio", EmbedMessageType.Error,
                        false, Context.User));
                    break;
                case Transfer.Result.bot:
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("", "Robôs não sabem usar dinheiro", EmbedMessageType.Confused,
                        false, Context.User));
                    break;
                case Transfer.Result.NotEnoughMiunies:
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("", "Gold insuficiente", EmbedMessageType.Error,
                        false, Context.User));
                    break;
                case Transfer.Result.Success:
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("", $"{Context.User.Mention}, você acaba de transferir {montante} gold(s) para {para.Mention}", EmbedMessageType.GoldGain,
                        false, Context.User));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Command("Apostar"), Remarks("Você aposta, e tem uma determinada de chance de ganhar")]
        public async Task ApostarTask(ulong aposta = 0)
        {
            //  Aposta, por padrão tem o valor de 0
            //  Sendo assim, caso o usuário não passe o parâmetro do valor da aposta
            //  Ela será 0, e o programa não irá retornar um erro por falta de parâmetros
            //  Não definimos como padrão "null" porque null não é um parâmetro válido
            //  para valores do tipo "ulong"

            //  Verificamos se a aposta é igual a 0
            if (aposta == 0)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("", "Você deve apostar um valor maior que ``0``", EmbedMessageType.Confused,
                        false, Context.User));
                return;
            }

            //  Obtemos o usuário
            var user = AccountsMananger.GetAccount(Context.User);

            //  Verificamos ele tem o valor valor que irá apostar
            if (aposta >= user.Gold)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("", $"Você não pode apostar mais do que você tem...\nPor favor, aposte um valor de até ``{user.Gold}``", EmbedMessageType.Error,
                        false, Context.User));
                return;
            }

            //  Geramos um valor de 1 a 100
            var r = new Random();
            var i = r.Next(1, 100);

            //  caso ele seja maior ou igual a 75
            //  Ou seja 75% é a porcentagem de vitória
            if (i <= Constantes.ChanceDeGanharAposta)
            {
                //  Se o if retornar verdadeiro, ganhou
                var lucro = aposta * 2;
                user.Gold = user.Gold + lucro;

                await ReplyAsync("",false,EmbedHandler.CriarEmbed("",$"Parabéns, você dobrou a sua aposta, e teve **{lucro} golds** de lucro.",EmbedMessageType.GoldGain,false,Context.User));
            }
            else
            {
                //  Se o if retornar falso, perdeu
                user.Gold = user.Gold - aposta;
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("", $"Você perdeu a sua aposta de **{aposta} golds**...\nAgora você tem **{user.Gold}**", EmbedMessageType.Error,false,Context.User));
            }

            AccountsMananger.SaveAccounts();
        }
    }
}