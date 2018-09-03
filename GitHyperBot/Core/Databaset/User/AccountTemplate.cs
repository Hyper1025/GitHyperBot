using System;

namespace GitHyperBot.Core.Databaset.User
{
    public class AccountTemplate
    {
        //  Id do usuário
        public string UserId { get; set; }

        //  Moeda
        public ulong Gold { get; set; }

        //  Ultimo Daily
        //  Já adicionamos um valor padrão, e removemos dois dias do dia atual
        public DateTime LastDaily { get; set; } = DateTime.UtcNow.AddDays(-2);

        public DateTime UltimaMensagem { get; set; } = DateTime.UtcNow;

        //  Spam
        public bool ReceberSpam { get; set; } = true;

        //  Registros
        public ulong MeRegistrouId { get; set; }
        public ulong RegistradosMasculino { get; set; }
        public ulong RegistradosFeminino { get; set; }

        //  Xp para sistema de experiência
        public ulong Xp { get; set; }

        //  Número de advertências
        public uint NumberOfWarning { get; set; }
    }
}