using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using GitHyperBot.Core.DataManipulation;

namespace GitHyperBot.Core.Databaset.User
{
    public static class AccountsMananger
    {
        private static readonly List<AccountTemplate> Accounts;
        private const string AccountsFile = "Resources/Accounts.json";

        static AccountsMananger()
        {
            //  Verificamos se o arquivo existe
            if (AccountsDataStorage.SaveAlreadyExists(AccountsFile))
            {
                //  Se existir carregamos
                //  Usando "LoadUsersAccounts"
                Accounts = AccountsDataStorage.LoadUsersAccounts(AccountsFile).ToList();
            }
            else
            {
                //  Do contrário, criamos
                Accounts = new List<AccountTemplate>();
                SaveAccounts();
            }
        }

        //  Salva os arquivo
        public static void SaveAccounts()
        {
            AccountsDataStorage.SaveUsersAccounts(Accounts, AccountsFile);
        }

        //  Isso é uma camada de "tradução" entre o usuário e o ID
        public static AccountTemplate GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        //  Obtém todas contas
        internal static List<AccountTemplate> GetAllAccounts()
        {
            return Accounts.ToList();
        }

        //  Obtém ou cria uma conta
        private static AccountTemplate GetOrCreateAccount(ulong id)
        {
            //  Procuramos pela conta
            //  Onde a conta selecionada deve ser a que
            //  O ID do arquivo e o ID solicitado sejam iguais
            //  Ao encontrar a conta, selecionamos ela
            var result = from a in Accounts where a.UserId == id select a;

            //  Atribuimos ela a variável
            var account = result.FirstOrDefault();

            //  Verificamos se existe a conta
            //  Caso nulo, criamos a conta
            if (account == null) account = CreateUserAccount(id);

            //  Retornamos a conta
            return account;
        }

        //  Método para criar conta não existente
        private static AccountTemplate CreateUserAccount(ulong id)
        {
            var newAccount = new AccountTemplate()
            {
                UserId = id
            };

            Accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}