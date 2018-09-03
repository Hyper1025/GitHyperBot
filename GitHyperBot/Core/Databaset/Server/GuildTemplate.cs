namespace GitHyperBot.Core.Databaset.Server
{
    public class GuildTemplate
    {
        //  Id do servidor <Key única>
        public ulong GuildId { get; set; }

        //  Maximo de warns
        //  Valor padrão definido como 4
        public uint MaxWarning { get; set; } = 4;

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

        //  Role Regras
        public ulong IdRoleRegras { get; set; }

        //  Role Registrado
        public ulong IdRoleRegistrado { get; set; }

        //  Role Não registrado
        public ulong IdRoleSemRegistro { get; set; }

        //  Role Masculino
        public ulong IdRoleMasculino { get; set; }

        //  Role Feminino
        public ulong IdRoleFemenino { get; set; }

        public bool TopicoChatGeralBool { get; set; }
        public string TopicoChatGeral { get; set; } = "{contador}";

        #region ChatGeral

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

        //  Tumb do usuário
        public bool BoasVindasTumbUsuario { get; set; }

        #endregion

        #region MensagemPrivada

        //  <Mensagem direta>
        //  Mensagem de boas vindas
        // (Liga, Desliga)
        public bool BoasVindasPvBool { get; set; }

        //  Título
        public string BoasVindasPvTitle { get; set; }

        //  Descrição
        public string BoasVindasPvDescription { get; set; }

        //  Imagem ou Gif URL
        public string BoasVindasPvUrl { get; set; }

        //  Tumb do usuário
        public bool BoasVindasPvTumbUsuario { get; set; }

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

        //  Delay auto-role
        public bool DelayAutoRole { get; set; }
        #endregion
    }
}