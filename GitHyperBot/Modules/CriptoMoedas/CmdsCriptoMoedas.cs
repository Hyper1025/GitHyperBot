using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Modules.CriptoMoedas.Dependencies;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.CriptoMoedas
{
    public class CmdsCriptoMoedas : ModuleBase<SocketCommandContext>
    {
        [Command("Bitcoin")]
        [Alias("Btc")]
        [Summary("Retorna informações sobre o bitcoin")]
        [CmdCategory(Categoria = CmdCategory.Ferramentas)]
        internal async Task BitcoinTask()
        {
            using (var http = new HttpClient())
            {
                var json = await http.GetStringAsync("https://api.coinmarketcap.com/v2/ticker/1/")
                    .ConfigureAwait(false);
                var dataFromJson = CoinmarketcapEntity.FromJson(json);
                var emb = new EmbedBuilder();

                emb.WithTitle("Cripto Moedas")
                    .WithColor(new Color(244, 188, 66))
                    .WithDescription("Aqui estão algumas informações sobre o Bitcoin")
                    .WithAuthor(Context.User.Username, Context.User.GetAvatarUrl())
                    .WithThumbnailUrl("http://icons.iconarchive.com/icons/paomedia/small-n-flat/512/bitcoin-icon.png")
                    .AddField("Símbolo", dataFromJson.Data.Symbol)
                    .AddField("Rank", dataFromJson.Data.Rank)
                    .AddField("Total", dataFromJson.Data.TotalSupply)
                    .AddField("Máximo", dataFromJson.Data.MaxSupply)
                    .AddField("Valor", $"{dataFromJson.Data.Quotes.Usd.Price} USD")
                    .WithCurrentTimestamp();

                await ReplyAsync("", false, emb);

            }
        }
    }
}