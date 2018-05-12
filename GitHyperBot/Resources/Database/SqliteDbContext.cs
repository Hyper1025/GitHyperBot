using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

//  Isso ainda pode ser completamente modificado no futuro, não sei se vai funcionar da maneira que eu espero...
//  Existindo a possibilidade de eu alterar isso tudo para arquivos .json
namespace GitHyperBot.Resources.Database
{
    public class SqliteDbContext : DbContext
    {
        //  Define pasta
        private const string DatabaseFolder = "Data";
        //  Define arquivo
        private const string DatabaseFile = "Database.sqlite";

        //  Define as tabelas
        //  Cria tabela chamada "TableAccounts", baseada na clase "Accounts"
        public DbSet<Accounts>TableAccounts { get; set; }
        //  Cria tabela chamada "TableGuilds", baseada na clase "Guilds"
        public DbSet<Guilds>TableGuilds { get; set; }

        //  Sobrescreve a configuração do EntityFrameworkCore
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //  Verifica a existência da pasta da base de dados
            if (!Directory.Exists(DatabaseFolder))
            {
                //  Se não existir, cria
                Directory.CreateDirectory(DatabaseFolder);
            }

            var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:\\", ""));

            //  Gera uma string com o local da base de dados
            string dbLocation = $"Data Source={DatabaseFolder}/{DatabaseFile}";

            //  Configura o local para conexão da tabela SQlite
            options.UseSqlite(dbLocation);
        }

        //  Sobescreve o parâmetro ao criar
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Define como valor padrão de "Mute" pertencente a classe Accounts como falso
            modelBuilder.Entity<Accounts>()
                .Property(x => x.Mute).HasDefaultValue(false);
        }
    }
}
