using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Modules.Admin.Dependencies;
using GitHyperBot.Modules.Admin.Dependencies.json;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Admin
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Command("Game")]
        [Alias("SetGame")]
        [Summary("Altera o que o bot está jogando atualmente.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [CmdCategory(Categoria = CmdCategory.Administração)]
        public async Task SetGameTask([Remainder] string nomeDoJogo)
        {
            await Context.Client.SetGameAsync(nomeDoJogo);
            await ReplyAsync("",false,EmbedHandler.CriarEmbed("Certo...",$"Jogo alterado para {nomeDoJogo}",EmbedMessageType.Config));
        }

        [Command("RegrasCriadas")]
        [Summary("Cria uma embed para o usuário concordar que leu os termos")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [CmdCategory(Categoria = CmdCategory.Administração)]
        internal async Task RegrasCriadasTask()
        {
            var guildAccount = GuildsMannanger.GetGuild(Context.Guild);

            if (guildAccount.IdRoleRegras == 0)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Opa...",
                        "Não posso fazer isso para você, se você ainda não definiu a role que será dada quando o usuário aceitar os as regras.\nFaça isso usando...",
                        EmbedMessageType.Info));
                await HelpService.HelpCommandTask(Context, "GC DefinirRoleRegras");
                return;
            }

            var m = await Context.Channel.SendMessageAsync("", false,
                EmbedHandler.CriarEmbed("Regras",
                    "Ao clicar em :white_check_mark:, você concorda que leu as regras e está de acordo com elas. O não cumprimento das mesmas resultará em medidas cabíveis conforme a situação.",
                    EmbedMessageType.Info));
            await m.AddReactionAsync(new Emoji("✅"));

            guildAccount.IdMsgRegras = m.Id;
            GuildsMannanger.SaveGuilds();

        }

        [Command("Reiniciar")]
        [Summary("Reinicia o bot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [CmdCategory(Categoria = CmdCategory.Administração)]
        internal async Task ResetTask()
        {
            //var guildAccount = GuildsMannanger.GetGuild(Context.Guild);
            //await ReplyAsync("", false, EmbedHandler.CriarEmbed("Reiniciando", "Isso não deve levar mais do que 30 segundos",EmbedMessageType.Info));

            try
            {
                var location = Assembly.GetExecutingAssembly().Location;
                var exe = location.Replace(".dll", ".exe");

                if (File.Exists(exe))
                {
                    Process.Start($"{exe}");
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("Sucesso", "Iniciando...\n" +
                                                                                   "Por favor, aguarde um momento.", EmbedMessageType.Success));
                    await Global.Client.LogoutAsync();
                    Environment.Exit(0);
                }
                else
                {
                    await ReplyAsync("", false, EmbedHandler.CriarEmbed("Ixi...", "Não encontrei o arquivo...\n" +
                                                                                   $"```{exe}```\n" +
                                                                                  $"Não foi encontrado", EmbedMessageType.Success));
                }
            }
            catch (Exception e)
            {
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Hiiii azedou", $"Não foi possível reiniciar ```{e}```", EmbedMessageType.Error));
                throw;
            }
        }

#if true
        [Command("Spam")]
        [Summary("Envia uma embed no privado de cada usuário")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [CmdCategory(Categoria = CmdCategory.Administração)]
        internal async Task SpamarPrivadoTask([Remainder] string json = null)
        {
            await Context.Message.DeleteAsync();
            if (json == null)
            {
                await ReplyAsync("Por favor, me envie um json no seguinte formato...\nVocê pode user {mencionar} para mencionar o usuário que receberá a mensagem", false,
                    EmbedHandler.CriarEmbed("Json",
                        "{\n \"Mensagem\": \"Digite aqui a mensagem que aparecerá fora da embed\",\n \"Titulo\": \"Digite aqui o título\",\n  \"Descrição\": \"Digite aqui a descrição\",\n  \"ImagemUrl\":\"UrlDaImagem.jpg\"\n}",
                        EmbedMessageType.Info, false));
                return;
            }

            await SpamService.SpamPrivado((SocketGuildUser)Context.User, json, (SocketTextChannel)Context.Channel);
        }

        [Command("TestSpam")]
        [Summary("Mostra como fica a embed do spam")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [CmdCategory(Categoria = CmdCategory.Administração)]
        internal async Task TestarSpamTask([Remainder] string json = null)
        {
            if (json == null)
            {
                await ReplyAsync("Por favor, me envie um json no seguinte formato", false,
                    EmbedHandler.CriarEmbed("Json",
                        "{\n \"Mensagem\": \"Digite aqui a mensagem que aparecerá fora da embed\",\n \"Titulo\": \"Digite aqui o título\",\n  \"Descrição\": \"Digite aqui a descrição\",\n  \"ImagemUrl\":\"UrlDaImagem.jpg\"\n}",
                        EmbedMessageType.Info, false));
                return;
            }
            var spamMessage = SpamMessage.FromJson(json);

            await ReplyAsync("Assim ficará a mensagem de spam:\n" +
                             $"{spamMessage.Mensagem.Replace("{mencionar}", Context.User.Mention)}", false,
                SpamService.RetornarEmbedSpam((SocketGuildUser)Context.User, json, (SocketGuildUser)Context.User));
        }
#endif
    }
}