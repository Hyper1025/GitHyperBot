using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Core;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Handlers;
using GitHyperBot.Core.Services;
using GitHyperBot.Modules.Admin.Dependencies;
using GitHyperBot.Modules.Help.Dependencies;

namespace GitHyperBot.Modules.Admin
{
    public class CmdsModeração : ModuleBase<SocketCommandContext>
    {
        [Command("Lock")]
        [Summary("Bloqueia o envio de mensagens do canal atual para o grupo everyone")]
        [RequireUserPermission(GuildPermission.ManageChannels), RequireUserPermission(GuildPermission.ManageChannels)]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        internal async Task LockTask()
        {
            var canal = (SocketGuildChannel) Context.Channel;

            //  Setamoms o nome da role
            const string roleName = "@everyone";

            await canal.AddPermissionOverwriteAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName),
                new OverwritePermissions(sendMessages: PermValue.Deny));

            await Context.Channel.SendMessageAsync("Canal Mutado");
            //  Enviamos o log
            await Logger.ChatLogTask(Logger.LogType.ChatLock,
                (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(Context.Guild).IdChatLog),Context.User, $"<#{Context.Channel.Id}>");
        }

        [Command("Unlock")]
        [Summary("Desbloqueia o envio de mensagens do canal atual para o grupo everyone")]
        [RequireUserPermission(GuildPermission.ManageChannels), RequireUserPermission(GuildPermission.ManageChannels)]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        internal async Task UnlockTask()
        {
            var canal = (SocketGuildChannel)Context.Channel;

            //  Setamoms o nome da role
            const string roleName = "@everyone";

            await canal.AddPermissionOverwriteAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName),
                new OverwritePermissions(sendMessages: PermValue.Allow));

            await Context.Channel.SendMessageAsync("Canal Desmutado");
            //  Enviamos o log
            await Logger.ChatLogTask(Logger.LogType.ChatUnlock,
                (SocketTextChannel)Global.Client.GetChannel(GuildsMannanger.GetGuild(Context.Guild).IdChatLog), Context.User, $"<#{Context.Channel.Id}>");
        }

        [Command("Limpar")]
        [Summary("Apaga uma quantidade determinada de mensagens.")]
        [RequireUserPermission(GuildPermission.ManageMessages), RequireBotPermission(ChannelPermission.ManageMessages)]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        internal async Task LimparChatTask(uint quantidade, SocketGuildUser usuário = null)
        {
            if (usuário == null)
            {
                await LimparService.LimparMensagensTask(Context.Channel as SocketTextChannel, quantidade);
            }
            else
            {
                await LimparService.LimparMensagensUsuário(Context.Channel as SocketTextChannel, quantidade,
                    usuário);
            }
        }

        [Command("Ban")]
        [Alias("Banir")]
        [Summary("Dá ban no usuário mencionado")]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        [RequireUserPermission(GuildPermission.BanMembers), RequireBotPermission(GuildPermission.BanMembers)]
        internal async Task BanirUsuarioTask(SocketGuildUser usuário, [Remainder]string motivo)
        {
            if (Context.User == usuário)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não pode fazer isso em sí mesmo.",
                        EmbedMessageType.AccessDenied, false, Context.User));
                return;
            }

            if (Context.Guild.GetUser(Context.User.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não tem permissão para banir usuários do seu mesmo cargo ou superior.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else if (Context.Guild.GetUser(Global.Client.CurrentUser.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sinto muito..", $"Eu não posso fazer isso com {usuário.Mention}, ele está a cima de mim em questão de hierarquia.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else
            {
                await BanService.BanirUsuario(usuário, Context.User as SocketGuildUser, Context.Guild, Context.Channel as SocketTextChannel, motivo);
            }    
        }

        [Command("Desban")]
        [Alias("Desbanir", "Unban")]
        [Summary("Remove o ban do usuário mencionado")]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        [RequireBotPermission(GuildPermission.BanMembers), RequireUserPermission(GuildPermission.BanMembers)]
        internal async Task DesbanirUsuárioTask(ulong idDoUsuário)
        {
            var bans = await Context.Guild.GetBansAsync();
            var selecionado = bans.FirstOrDefault(x => x.User.Id == idDoUsuário);

            if (selecionado != null)
            {
                await Context.Guild.RemoveBanAsync(selecionado.User);
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Desbanido", $"O usuário de id: ``{idDoUsuário}`` foi desbanido", EmbedMessageType.Success, false));
            }
            else
            {
                await ReplyAsync("", false, EmbedHandler.CriarEmbed("Ops...", $"O ban não existe para o id ``{idDoUsuário}``", EmbedMessageType.Error, false));
            }

        }

        [Command("MotivoBan")]
        [Alias("BanReason", "Razão Do Ban", "Motivo Do Ban", "motivo ban")]
        [Summary("Diz a razão do ban para o ID mencionado")]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        internal async Task RazãoDoBanTask(ulong idDoUsuário)
        {
            var bans = await Context.Guild.GetBansAsync();
            var selecionado = bans.FirstOrDefault(x => x.User.Id == idDoUsuário);

            if (selecionado != null)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Razão Do Ban",
                        $"A razão do ban do usuário de id ``{idDoUsuário}``, foi ```{selecionado.Reason}```",
                        EmbedMessageType.Info, false));
            }
            else
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Opa...",
                        $"Não encontrei nenhum ban para o id ``{idDoUsuário}``",
                        EmbedMessageType.Error, false));
            }

        }

        [Command("Mute")]
        [Alias("Mutar")]
        [Summary("Muta o usuário mencionado")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        internal async Task MutarUsuárioTask(SocketGuildUser usuário)
        {
            if (Context.User == usuário)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não pode fazer isso em sí mesmo.",
                        EmbedMessageType.AccessDenied, false, Context.User));
                return;
            }

            if (Context.Guild.GetUser(Context.User.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não tem permissão para mutar usuários do seu mesmo cargo ou superior.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else if (Context.Guild.GetUser(Global.Client.CurrentUser.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sinto muito..", $"Eu não posso fazer isso com {usuário.Mention}, ele está a cima de mim em questão de hierarquia.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else
            {
                await MuteService.MutarTask(usuário, Context.Channel);
            }
        }

        [Command("Desmute")]
        [Alias("Desmutar", "Unmute")]
        [Summary("Muta o usuário mencionado")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        internal async Task DesmutarUsuárioTask(SocketGuildUser usuário)
        {
            if (Context.User == usuário)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não pode fazer isso em sí mesmo.",
                        EmbedMessageType.AccessDenied, false, Context.User));
                return;
            }

            if (Context.Guild.GetUser(Context.User.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não tem permissão para desmutar usuários do seu mesmo cargo ou superior.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else if (Context.Guild.GetUser(Global.Client.CurrentUser.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sinto muito..", $"Eu não posso fazer isso com {usuário.Mention}, ele está a cima de mim em questão de hierarquia.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else
            {
                await MuteService.DesmutarTask(usuário, Context.Channel);
            }
        }

        [Command("Warn")]
        [Alias("Warnar")]
        [Summary("Adverte um usuário")]
        [RequireBotPermission(GuildPermission.BanMembers), RequireUserPermission(GuildPermission.BanMembers)]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        internal async Task WarnarUsuárioTask(SocketGuildUser usuário)
        {
            if (Context.User == usuário)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não pode fazer isso em sí mesmo.",
                        EmbedMessageType.AccessDenied, false, Context.User));
                return;
            }

            if (Context.Guild.GetUser(Context.User.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não tem permissão para warnar usuários do seu mesmo cargo ou superior.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else if (Context.Guild.GetUser(Global.Client.CurrentUser.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sinto muito..", $"Eu não posso fazer isso com {usuário.Mention}, ele está a cima de mim em questão de hierarquia.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else
            {
                await WarnService.AdicionarWarnTask(usuário, (SocketGuildUser)Context.User, Context.Channel);
            }
        }

        [Command("TirarWarns")]
        [Alias("Deswarnar")]
        [Summary("Remove um determinado número de warns")]
        [RequireBotPermission(GuildPermission.BanMembers), RequireUserPermission(GuildPermission.BanMembers)]
        [CmdCategory(Categoria = CmdCategory.Moderação)]
        internal async Task TirarWarnsUsuárioTask(uint numeroDeWarns, SocketGuildUser usuário)
        {
            if (Context.User == usuário)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não pode fazer isso em sí mesmo.",
                        EmbedMessageType.AccessDenied, false, Context.User));
                return;
            }

            if (Context.Guild.GetUser(Context.User.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Acho que não, em...",
                        "Você não tem permissão para tirar warns de usuários do seu mesmo cargo ou superior.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else if (Context.Guild.GetUser(Global.Client.CurrentUser.Id).Hierarchy <= usuário.Hierarchy)
            {
                await ReplyAsync("", false,
                    EmbedHandler.CriarEmbed("Sinto muito..", $"Eu não posso fazer isso com {usuário.Mention}, ele está a cima de mim em questão de hierarquia.",
                        EmbedMessageType.AccessDenied, false, Context.User));
            }
            else
            {
                await WarnService.TirarWarnTask(usuário, Context.Channel,numeroDeWarns);
            }
        }
    }
}