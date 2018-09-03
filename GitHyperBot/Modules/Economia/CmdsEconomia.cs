using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Economia.Dependencies;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Economia
{
    //  <Sumary>
    //  Comandos relacionados ao sistema de economia
    //  </Sumary>

    public class CmdsEconomia : ModuleBase<SocketCommandContext>
    {
        [Command("Daily")]
        [Alias("GetDaily", "ClaimDaily", "GetGold", "ClaimGold")]
        [Summary("Dá uma quantidade de gold por dia.")]
        [CmdCategory(Categoria = CmdCategory.Economia)]
        public async Task GetDailyTask()
        {
            //  Chama o método "GetDailyResult" passando o usuário que executou o comando
            //  Atribui o retorno do método a variável resultado
            var resultado = Dayli.GetDailyResult((SocketGuildUser)Context.User);

            //  Verifica o resultado
            if (resultado.Sucesso)
            {
                //  Caso sucesso for verdadeiro
                await ReplyAsync(Context.User.Mention, false,
                    EmbedHandler.CriarEmbed("Boa", $"Você ganhou {Constantes.DailyGold} golds",
                        EmbedMessageType.GoldGain, false, Context.User));
            }
            else
            {
                //  Caso sucesso for falso
                var tempo = string.Format("{0:%h} horas {0:%m} minutos {0:%s} segundos", resultado.IntervaloDeTempo);

                await ReplyAsync(Context.User.Mention, false,
                    EmbedHandler.CriarEmbed("",
                        $"Você já pegou o seu **Gold** diário.\nVolte novamente em **{tempo}**",
                        EmbedMessageType.Info, false, Context.User));
            }
        }

        [Command("Golds")]
        [Alias("Gold","Carteira")]
        [Summary("Mostra quantos golds você tem.")]
        [CmdCategory(Categoria = CmdCategory.Economia)]
        public async Task ChecarGoldsTask()
        {
            var account = AccountsMananger.GetAccount(Context.User, Context.Guild);
            await ReplyAsync(Context.User.Mention, false,
                EmbedHandler.CriarEmbed("Aqui está a sua carteira", $"Você tem **{account.Gold} golds**", EmbedMessageType.GoldGain, false, Context.User));
        }

        [Command("Transferir")]
        [Summary("Transfere dinheiro para outra pessoa")]
        [CmdCategory(Categoria = CmdCategory.Economia)]
        public async Task TransferirGoldTask(IGuildUser para, ulong montante)
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
                case Transfer.Result.Bot:
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("", "Robôs não sabem usar dinheiro", EmbedMessageType.Confused,
                        false, Context.User));
                    break;
                case Transfer.Result.NotEnoughGold:
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

        [Command("Apostar")]
        [Summary("Você aposta, e tem uma determinada de dobrar a sua aposta")]
        [CmdCategory(Categoria = CmdCategory.Economia)]
        public async Task ApostarTask(ulong aposta)
        {
            //  Aposta, por padrão tem o valor de 0
            //  Sendo assim, caso o usuário não passe o parâmetro do valor da aposta
            //  Ela será 0, e o programa não irá retornar um erro por falta de parâmetros
            //  Não definimos como padrão "null" porque null não é um parâmetro válido
            //  para valores do tipo "ulong"

            //  Verificamos se a aposta é igual a 0
            if (aposta == 0)
            {
                await ReplyAsync("Poxa...", false,
                    EmbedHandler.CriarEmbed("", "Você deve apostar um valor maior que ``0``", EmbedMessageType.Confused,
                        false, Context.User));
                return;
            }

            //  Obtemos o usuário
            var user = AccountsMananger.GetAccount(Context.User, Context.Guild);

            //  Verificamos ele tem o valor valor que irá apostar
            if (aposta >= user.Gold)
            {
                await ReplyAsync(Context.User.Mention, false,
                    EmbedHandler.CriarEmbed("Hi...", $"Você não pode apostar mais do que você tem...\nVocê tem ``{user.Gold}`` golds", EmbedMessageType.Error,
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

                await ReplyAsync(Context.User.Mention, false,EmbedHandler.CriarEmbed("BOA!",$"Parabéns, você dobrou a sua aposta, e teve **{lucro} golds** de lucro.",EmbedMessageType.GoldGain,false,Context.User));
            }
            else
            {
                //  Se o if retornar falso, perdeu
                user.Gold = user.Gold - aposta;
                await ReplyAsync(Context.User.Mention, false,
                    EmbedHandler.CriarEmbed("Que azar...", $"Você perdeu a sua aposta de **{aposta} golds**...\nAgora você tem **{user.Gold}**", EmbedMessageType.GoldLose,false,Context.User));
            }

            AccountsMananger.SaveAccounts();
        }

        [Command("NovaMaquina")]
        [Alias("newslot")]
        [Summary("Cria uma nova máquina caça-níqueis se você se sentir azarado")]
        [CmdCategory(Categoria = CmdCategory.Economia)]
        public async Task NewSlotTask(int montante = 0)
        {
            Global.Slot = new Slot(montante);
            await ReplyAsync($"{Context.User.Mention} Uma nova slot machine foi gerada! Boa sorte!");
        }

        [Command("Slots")]
        [Alias("Slot", "caça-níqueis")]
        [Summary("Jogue, ganhe ou perca alguns golds")]
		[CmdCategory(Categoria = CmdCategory.Economia)]
        public async Task JogarSlotTask(uint quantidade)
        {
            if (quantidade < 1)
            {
                await ReplyAsync("",false,EmbedHandler.CriarEmbed("É...", "Você não pode jogar com essa quantia de gold.\nE VOCÊ SABE DISSO...",EmbedMessageType.AccessDenied,false));
                return;
            }
            var account = AccountsMananger.GetAccount(Context.User, Context.Guild);
            if (account.Gold < quantidade)
            {
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("É...", $"Desculpe, mas você não tem golds o suficientes... Você só tem {account.Gold}.", EmbedMessageType.AccessDenied, false));
                return;
            }

            account.Gold -= quantidade;
            AccountsMananger.SaveAccounts();

            var slotEmojis = Global.Slot.Spin();
            var payoutAndFlavour = Global.Slot.GetPayoutAndFlavourText(quantidade);

            if (payoutAndFlavour.Item1 > 0)
            {
                account.Gold += payoutAndFlavour.Item1;
                AccountsMananger.SaveAccounts();
            }

            var emb = new EmbedBuilder();
            var r = new Random();

            emb.WithTitle("🎰 Máquina")
                .WithAuthor(Context.User.Username,
                    Context.User.GetAvatarUrl() ?? $"https://cdn.discordapp.com/embed/avatars/{r.Next(0, 4)}.png")
                .WithColor(Color.Default)
                .WithDescription(slotEmojis);

            var m = await ReplyAsync(Context.User.Mention, false, emb);
            //await ReplyAsync(slotEmojis);
            await Task.Delay(1000);

            await m.ModifyAsync(x => x.Content = $"{Context.User.Mention} {payoutAndFlavour.Item2}");
            emb.WithColor(payoutAndFlavour.Item3);

            await m.ModifyAsync(x => x.Embed = new Optional<Embed>(emb));
            //await ReplyAsync(payoutAndFlavour.Item2);
        }
    }
}