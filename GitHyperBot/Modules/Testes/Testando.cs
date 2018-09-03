#if DEBUG
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Services;

namespace GitHyperBot.Modules.Testes
{
    public class Testando : ModuleBase<SocketCommandContext>
    {
        //[Command("DRepetir")]
        //[RequireUserPermission(GuildPermission.Administrator)]
        //internal async Task DRepetirTask([Remainder] string texto)
        //{
        //    var emb = new EmbedBuilder();

        //    emb.WithTitle("Debug").WithDescription($"```{texto}```");
        //    await ReplyAsync("", false, emb);
        //}

        [Command("test")]
        public async Task TestandoTask()
        {
            if (Context.User.Id != 152575748861984768)
            {
                return;
            }

            var guild = Context.Guild;
            var roleAntiga = guild.Roles.FirstOrDefault(x => x.Id == 461327060607041536); //    Mortais
            var roleNova = guild.Roles.FirstOrDefault(x => x.Id == 466386329408765953);    //  Registre-me

            foreach (var guildUser in guild.Users.Where(x=> x.IsBot == false && x.Roles.Count <= 1))
            {
                    if (guildUser.Roles.Contains(roleAntiga))
                    {
                        await guildUser.RemoveRoleAsync(roleAntiga);
                        //await Task.Delay(2000);
                    }

                    try
                    {
                        await guildUser.AddRoleAsync(roleNova);
                        Console.WriteLine($"G G          Sucesso -> {guildUser.Username}");
                    //await Context.Channel.SendMessageAsync($":white_check_mark: Sucesso -> {guildUser.Mention}");
                }
                    catch (Exception)
                    {
                        Console.WriteLine($"ERRO          ERRO -> {guildUser.Username}");
                        //await Context.Channel.SendMessageAsync($":x: Erro -> {guildUser.Mention}");
                    }
            }

            await Context.Channel.SendMessageAsync($":white_check_mark: Concluido");
        }
    }
}
#endif