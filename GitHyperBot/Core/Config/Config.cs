using System.IO;
using Newtonsoft.Json;

namespace GitHyperBot.Core.Config
{
    internal class Config
    {
        //  <summary>
        //  Clase responsável por arquivos de configuração
        //  base do bot, como token e prefixo de comando
        //  </summary>

        //  Pasta
        private const string ConfigFolder = "Resources";
        //  Arquivo de config
        private const string ConfigFile = "Data.json";

        //  Servirá para se referir as configs
        //  Isso é básicamente uma propiedade da classe BotConfig
        //  Que está definida no final
        //  "Bot" tem Token e CmdPrefix
        public static BotConfig Bot;


        static Config()
        {
            //  Verifica existência da pasta de configuração
            if (!Directory.Exists(ConfigFolder))
            {
                //  Cria a pasta de configuração
                Directory.CreateDirectory(ConfigFolder);
            }

            //  Verifica existência do arquivo
            if (!File.Exists($"{ConfigFolder}/{ConfigFile}"))
            {
                //  Se o arquivo não existir, vamos criá-lo

                //  Cria uma instância das propiedades apontadas pela clase BotConfig
                Bot = new BotConfig();

                //  Convertemos a classe bot em um arquivo Json
                //  Definimos o objeto a ser convertido, no caso "Bot"
                //  Definimos um parâmetro opcional, porém muito útil
                //  o a formatação no caso, "Formatting.Indented"
                //  Isso fará com que a formatação do arquivo seja mais amigável a leitura
                //  Isso é, as chaves do arquivo json, junto ao seu parâmetro, não ficarão
                //  em apenas uma linha única
                //  E sim em várias linhas, uma para cada chave
                string json = JsonConvert.SerializeObject(Bot,Formatting.Indented);

                //  Escrevemos o arquivo
                //  Declarando o parâmetro do lugar
                //  E do proprio arquivo já formatado para json
                File.WriteAllText($"{ConfigFolder}/{ConfigFile}",json);
            }
            else
            {
                //  Caso o arquivo de configuração já exista

                //  Aqui faremos o oposto de criar, iremos ler o arquivo
                string json = File.ReadAllText($"{ConfigFolder}/{ConfigFile}");

                //  Aqui nos "desconvertemos" o arquivo json, para leitura do programa
                //  Apontando "BotConfig" como uma "forma" para leitura dos parâmetros
                //  E apontando "Json" como a string a ser lida
                //  E por útimo atribuimos a leitura desse arquivo ao objeto Bot
                Bot = JsonConvert.DeserializeObject<BotConfig>(json);


            }
        }
    }

    public struct BotConfig
    {
        public string Token { get; set; }
        public string CmdPrefix { get; set; }
        public string GiphyApiKey { get; set; }
        public string LoLApiKey { get; set; }
    }
}