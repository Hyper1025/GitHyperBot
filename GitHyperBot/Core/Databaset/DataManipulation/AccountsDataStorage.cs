using System.Collections.Generic;
using System.IO;
using GitHyperBot.Core.Databaset.User;
using Newtonsoft.Json;

namespace GitHyperBot.Core.Databaset.DataManipulation
{
    public static class AccountsDataStorage
    {
        //  Salvar todas contas
        //  Criamos uma classe publica e estática
        //  que não retorna nada
        //  Ela precisa de um argumento, que é o IEnumerable
        //  Que basicamente é uma interface de coisas que você pode contar
        //  E nos queremos uma interface contável de contas (accounts, que é a classe)
        public static void SaveUsersAccounts(IEnumerable<AccountTemplate> accounts, string filePath)
        {
            //  Geramos o arquivo
            var json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(filePath, json);
        }

        public static IEnumerable<AccountTemplate> LoadUsersAccounts(string filePath)
        {
            //  Se o arquivo não existir, retornamos nulo
            if (!File.Exists(filePath)) return null;

            //  Caso contrário, seguimos
            //  Lemos o arquivo para a string json
            var json = File.ReadAllText(filePath);

            //  Retornamos ele como uma lista "IEnumerable<AccountTemplate>"
            return JsonConvert.DeserializeObject<List<AccountTemplate>>(json);
        }

        //  Verifica se o save já existe
        //  Só temos essa função para não puxar o System.io para outra classe
        public static bool SaveAlreadyExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}