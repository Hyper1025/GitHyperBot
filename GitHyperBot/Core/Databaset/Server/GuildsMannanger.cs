using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using GitHyperBot.Core.DataManipulation;

namespace GitHyperBot.Core.Databaset.Server
{
    public static class GuildsMannanger
    {
        private static readonly List<GuildTemplate> Guilds;
        private const string GuildsFile = "Resources/Guilds.json";

        static GuildsMannanger()
        {
            //  Verificamos se o arquivo existe
            if (GuildsDataStorage.SaveAlreadyExists(GuildsFile))
            {
                //  Se existir carregamos
                //  Usando "LoadUsersAccounts"
                Guilds = GuildsDataStorage.LoadGuildAccounts(GuildsFile).ToList();
            }
            else
            {
                //  Do contrário, criamos
                Guilds = new List<GuildTemplate>();
                SaveGuilds();
            }
        }

        //  Salva os arquivo
        public static void SaveGuilds()
        {
            GuildsDataStorage.SaveGuildAccounts(Guilds, GuildsFile);
        }

        //  Isso é uma camada de "tradução" entre a guild e o ID
        public static GuildTemplate GetGuild(SocketGuild guild)
        {
            return GetOrCreateGuild(guild.Id);
        }

        //  Obtém ou cria uma conta
        private static GuildTemplate GetOrCreateGuild(ulong id)
        {
            //  Procuramos pela conta
            //  Onde a conta selecionada deve ser a que
            //  O ID do arquivo e o ID solicitado sejam iguais
            //  Ao encontrar a conta, selecionamos ela
            var result = from g in Guilds where g.GuildId == id select g;

            //  Atribuimos ela a variável
            var guild = result.FirstOrDefault();

            //  Verificamos se existe a conta
            //  Caso nulo, criamos a conta
            if (guild == null) guild = CreateUserAccount(id);

            //  Retornamos a conta
            return guild;
        }

        //  Método para criar guild não existente
        private static GuildTemplate CreateUserAccount(ulong id)
        {
            var newAccount = new GuildTemplate()
            {
                GuildId = id
            };

            Guilds.Add(newAccount);
            SaveGuilds();
            return newAccount;
        }
    }
}