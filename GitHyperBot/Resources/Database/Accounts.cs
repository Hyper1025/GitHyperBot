using System.ComponentModel.DataAnnotations;

namespace GitHyperBot.Resources.Database
{
    public class Accounts
    {
        //  Chave do banco de dados
        [Key]
        public ulong UserId { get; set; }   //  Id do usuário

        //  Pontos para possível sistema de economia
        public uint Points { get; set; }

        //  Xp para sistema de experiência
        public uint Xp { get; set; }

        //  Número de advertências
        public uint NumberOfWarning { get; set; }

        //  Estado de mute do usuário
        public bool Mute { get; set; }
    }
}