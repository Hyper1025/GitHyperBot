using System;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Databaset.User;

namespace GitHyperBot.Modules.Economia.Dependencies
{
    public static class Dayli
    {
        //  Estrutura para encapsular duas variáveis relacionadas
        public struct DailyResult
        {
            public bool Sucesso;
            public TimeSpan IntervaloDeTempo;
        }

        //  Método
        public static DailyResult GetDailyResult(SocketGuildUser user)
        {
            //  Obtém a conta
            var account = AccountsMananger.GetAccount(user,user.Guild);

            //  Calcula a diferença de tempo, desde o último Daily
            var diferença = DateTime.UtcNow - account.LastDaily.AddDays(1);

            //  Se a diferença for menor que 0
            //  o GetDailyResult não foi um sucesso.
            //  Sendo assim retornamos os dois parâmetros
            //  Sucesso sendo falso, e o intervalo de tempo necessário
            //  até o próximo  comando ser aceito com sucesso
            if (diferença.TotalHours < 0)
            {
                return new DailyResult {Sucesso = false, IntervaloDeTempo = diferença };
            }

            //  Sempre que retornamos algo, o programa sai do método
            //  Não percorrendo o resto, sendo assim, se elimina a necessidade de usar um else
            //  Já que essa parte do código só será executada, caso não entre dentro do if

            //  Acrescentamos o gold para a conta
            account.Gold += Constantes.DailyGold;
            //  Atualizamos o horário, dês de o ultimo daily
            account.LastDaily = DateTime.UtcNow;
            //  Salvamos a conta
            AccountsMananger.SaveAccounts();
            //  Retornamos o método com sucesso sendo verdadeiro
            return new DailyResult{Sucesso = true};
        }
    }
}