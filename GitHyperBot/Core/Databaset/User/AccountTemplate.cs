using System;

namespace GitHyperBot.Core.Databaset.User
{
    public class AccountTemplate
    {
        //  Id do usuário
        public ulong UserId { get; set; }

        //  Moeda
        public ulong Gold { get; set; }

        //  Ultimo Daily
        //  Já adicionamos um valor padrão, e removemos dois dias do dia atual
        public DateTime LastDaily { get; set; } = DateTime.UtcNow.AddDays(-2);

        //  Xp para sistema de experiência
        public uint Xp { get; set; }

        //  Número de advertências
        public uint NumberOfWarning { get; set; }
    }
}