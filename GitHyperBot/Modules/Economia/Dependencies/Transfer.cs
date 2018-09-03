using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.User;

namespace GitHyperBot.Modules.Economia.Dependencies
{
    public class Transfer
    {
        internal enum Result
        {
            Success, SelfTransfer, NotEnoughGold, Bot
        }

        internal static Result DeUserParaUseResult(IUser de, IUser para, ulong montanteUlong)
        {
            //  Verifica se foi a transferência foi realizada para a mesma pessoa
            if (de.Id == para.Id) return Result.SelfTransfer;
            if (para.IsBot) return Result.Bot;

            var fonte = AccountsMananger.GetAccount((SocketUser) de, ((SocketGuildUser) de).Guild);

            if (fonte.Gold < montanteUlong) return Result.NotEnoughGold;

            var destino = AccountsMananger.GetAccount((SocketUser) para ,((SocketGuildUser)para).Guild);

            fonte.Gold -= montanteUlong;
            destino.Gold += montanteUlong;

            AccountsMananger.SaveAccounts();

            return Result.Success;

        }
    }
}