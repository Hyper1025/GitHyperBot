using System.Threading.Tasks;
using Discord.Commands;
//using GitHyperBot.Core.Databaset.User;

namespace GitHyperBot.Modules
{
    //  <summary>
    //  Essa classe contém comandos
    //  que pódem serem executados
    //  por usuários
    //  </summary>
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("olá"), Alias("oi"), Summary("Retorna uma mensagem de olá")]
        public async Task OláTask()
        {
            await ReplyAsync($"Olá {Context.User.Mention}");
        }

        //[Command("test")]
        //public async Task TesTask(uint n = 0)
        //{
        //    var account = AccountsMananger.GetAccount(Context.User);

        //    if (n == 0)
        //    {
        //        await ReplyAsync(account.Xp.ToString());
        //    }
        //    else
        //    {
        //        account.Xp += n;
        //        AccountsMananger.SaveAccounts();
        //        await ReplyAsync(account.Xp.ToString());
        //    }
        //}
    }
}