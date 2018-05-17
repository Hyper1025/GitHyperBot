using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Databaset.User;

namespace GitHyperBot.Features.Economia
{
    public class Transfer
    {
        internal enum Result
        {
            Success, SelfTransfer, NotEnoughMiunies, bot
        }

        internal static Result DeUserParaUseResult(IUser de, IUser para, ulong montanteUlong)
        {
            //  Verifica se foi a transferência foi realizada para a mesma pessoa
            if (de.Id == para.Id) return Result.SelfTransfer;
            if (para.IsBot) return Result.bot;

            var fonte = AccountsMananger.GetAccount((SocketUser)de);

            if (fonte.Gold < montanteUlong) return Result.NotEnoughMiunies;

            var destino = AccountsMananger.GetAccount((SocketUser) para);

            fonte.Gold -= montanteUlong;
            destino.Gold += montanteUlong;

            AccountsMananger.SaveAccounts();

            return Result.Success;

        }
    }
}