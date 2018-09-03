using System.Collections.Generic;
using System.IO;
using GitHyperBot.Core.Databaset.Server;
using Newtonsoft.Json;

namespace GitHyperBot.Core.Databaset.DataManipulation
{
    public static class GuildsDataStorage
    {
        //  Mesma coisa que a AccountsDataStorage, só que para as guilds

        public static void SaveGuildAccounts(IEnumerable<GuildTemplate> accounts, string filePath)
        {
            //  Geramos o arquivo
            var json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static IEnumerable<GuildTemplate> LoadGuildAccounts(string filePath)
        {
            //  Se o arquivo não existir, retornamos nulo
            if (!File.Exists(filePath)) return null;

            //  Caso contrário, seguimos
            //  Lemos o arquivo para a string json
            var json = File.ReadAllText(filePath);

            //  Retornamos ele como uma lista "IEnumerable<GuildTemplate>"
            return JsonConvert.DeserializeObject<List<GuildTemplate>>(json);
        }

        //  Verifica se o save já existe
        //  Só temos essa função para não puxar o System.io para outra classe
        public static bool SaveAlreadyExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}