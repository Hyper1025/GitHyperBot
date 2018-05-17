namespace GitHyperBot.Core.Databaset.Server
{
    public class GuildTemplate
    {
        //  Id do servidor
        public ulong GuildId { get; set; }

        //  Maximo de warns
        public uint MaxWarning { get; set; }

        //  Id do chat geral
        public ulong IdChatGeral { get; set; }

        //  Id do chat de log
        public ulong IdChatLog { get; set; }

        //  Id do chat de convites
        public ulong IdChatConvites { get; set; }

        //  Bloquear convites
        public bool BlockInvites { get; set; }

        //  Id mensagem registro
        public ulong IdMsgRegistro { get; set; }

        //  Id mensagem regras
        public ulong IdMsgRegras { get; set; }

        //  <CHAT GERAL>
        //  Mensagem de boas vindas chat geral
        //  (Liga, Desliga)
        public bool BoasVindasBool { get; set; }

        //  Título
        public string BoasVindasTitle { get; set; }

        //  Descrição
        public string BoasVindasDescription { get; set; }

        //  Imagem ou Gif URL
        public string BoasVindasUrl { get; set; }

        //  <PRIVADO>
        //  Mensagem de boas vindas
        // (Liga, Desliga)
        public bool BoasVindasPvBool { get; set; }

        //  Título
        public string BoasVindasPvTitle { get; set; }

        //  Descrição
        public string BoasVindasPvDescription { get; set; }

        //  Imagem ou Gif URL
        public string BoasVindasPvUrl { get; set; }

        //  Campo 1
        //  Título
        public string BoasVindasPvField1Title { get; set; }
        //  Descrição
        public string BoasVindasPvField1Descri { get; set; }

        //  Campo 2
        //  Título
        public string BoasVindasPvField2Title { get; set; }
        //  Descrição
        public string BoasVindasPvField2Descri { get; set; }

        //  Campo 3
        //  Título
        public string BoasVindasPvField3Title { get; set; }
        //  Descrição
        public string BoasVindasPvField3Descri { get; set; }

        //  Rodapé
        public string BoasVindasPvFooter { get; set; }
    }
}