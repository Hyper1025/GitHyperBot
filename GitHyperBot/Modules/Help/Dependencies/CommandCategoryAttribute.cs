using System;

namespace GitHyperBot.Modules.Help.Dependencies
{
    public class CmdCategoryAttribute : Attribute
    {
        public CmdCategory Categoria;
    }

    public enum CmdCategory
    {
        Geral,
        Administração,
        Moderação,
        Configurações,
        Ferramentas,
        Diversão,
        Economia,
        Nsfw,
        Misc,
        Divulgador,
        Registrador
    }
}