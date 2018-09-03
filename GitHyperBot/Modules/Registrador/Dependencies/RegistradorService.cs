using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Databaset.Server;
using GitHyperBot.Core.Databaset.User;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules.Registrador.Dependencies
{
    public class RegistradorService
    {
        internal static async Task RegistradorTask(SocketGuildUser registrador, SocketGuildUser usuário, SocketTextChannel canal)
        {
            //if (registrador == usuário)
            //{
            //    await canal.SendMessageAsync("", false,
            //        EmbedHandler.CriarEmbed("Ops...",
            //            $"Você não pode fazer isso em sí próprio.",
            //            EmbedMessageType.Error, false));
            //}
            var registradorAccount = AccountsMananger.GetAccount(registrador, registrador.Guild);
            var usuárioAccount = AccountsMananger.GetAccount(usuário, usuário.Guild);
            var guild = registrador.Guild;
            var guildAccount = GuildsMannanger.GetGuild(guild);

            //  Verifica se as role foi configurada
            if (guildAccount.IdRoleFemenino == 0 || guildAccount.IdRoleMasculino == 0 || guildAccount.IdRoleRegistrado == 0 || guildAccount.IdRoleSemRegistro == 0)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...",
                        "Confira se as roles referente a sexo masculino e feminino, e a role de registro foram configuradas",
                        EmbedMessageType.Error, false));
                return;
            }

            //  Obtemos as roles
            var roleMasculino = guild.Roles.FirstOrDefault(x=> x.Id == guildAccount.IdRoleMasculino);
            var roleFemenino = guild.Roles.FirstOrDefault(x => x.Id == guildAccount.IdRoleFemenino);
            var roleRegistrado = guild.Roles.FirstOrDefault(x => x.Id == guildAccount.IdRoleRegistrado);

            if (roleMasculino == null)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Tem algo errado",
                        $"A role referente ao sexo **Masculino**, está mal configurada... por favor, reconfigure usando ```{Config.Bot.CmdPrefix}Gc DefinirRoleMasculino <role>```",
                        EmbedMessageType.Error, false));
                return;
            }

            if (roleFemenino == null)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Tem algo errado",
                        $"A role referente ao sexo Femenino, está mal configurada... por favor, reconfigure usando ```{Config.Bot.CmdPrefix}Gc DefinirRoleFeminino <role>```",
                        EmbedMessageType.Error, false));
                return;
            }

            if (roleRegistrado == null)
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Tem algo errado",
                        $"A role referente a confirmação de registro, está mal configurada... por favor, reconfigure usando ```{Config.Bot.CmdPrefix}Gc DefinirRoleRegistrado <role>```",
                        EmbedMessageType.Error, false));
                return;
            }


            //  Verifica se o usuário já foi registrado
            if (usuário.Roles.Contains(roleRegistrado))
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...",
                        $"O usuário {usuário.Mention} já foi registrado",
                        EmbedMessageType.Error, false));
                return;
            }

            //  Verificamos caso o usuário tem as duas roles
            if (usuário.Roles.Contains(roleFemenino) && usuário.Roles.Contains(roleMasculino))
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...",
                        $"O usuário {usuário.Mention} não pode ter as roles masculino e feminino ao mesmo tempo",
                        EmbedMessageType.Error, false));
                return; //  Se tiver retornamos
            }

            //  Verificamos se o usuário tem alguma das duas
            //  <eu sei que dá pra inverter o if, mas é toque>
            if (usuário.Roles.Contains(roleFemenino) || usuário.Roles.Contains(roleMasculino))
            {

            }
            else
            {
                await canal.SendMessageAsync("", false,
                    EmbedHandler.CriarEmbed("Ops...",
                        $"O usuário{usuário.Mention} não tem nenhuma das roles Masculino ou Feminino.",
                        EmbedMessageType.Error, false));
                return;
            }


            var sexo = string.Empty;

            //  Verificamos se ele tem a role feminino
            if (usuário.Roles.Contains(roleFemenino))
            {
                await usuário.AddRoleAsync(roleFemenino);
                registradorAccount.RegistradosFeminino++;
                sexo = "Feminino";
            }
            //  Verificamos se ele tem a role masculino
            else if (usuário.Roles.Contains(roleMasculino))
            {
                await usuário.AddRoleAsync(roleMasculino);
                registradorAccount.RegistradosMasculino++;
                sexo = "Masculino";
            }

            //  Registramos o usuário
            var removerRole = guild.Roles.FirstOrDefault(x => x.Id == guildAccount.IdRoleSemRegistro);
            await usuário.AddRoleAsync(roleRegistrado);
            await usuário.RemoveRoleAsync(removerRole);

            //  Salvamos quem registrou o usuário
            usuárioAccount.MeRegistrouId = registrador.Id;
            AccountsMananger.SaveAccounts();

            await usuário.SendMessageAsync("", false, EmbedHandler.CriarEmbed("", $"Obrigado por se registar com {registrador.Mention}.\nVocê foi registrado no sexo {sexo}.", EmbedMessageType.Success, false, usuário));
            await canal.SendMessageAsync("", false,
                EmbedHandler.CriarEmbed("Registrado",
                    $"O usuário {usuário.Mention}, foi registrado por {registrador.Mention}, com sucesso.",
                    EmbedMessageType.Success, false));
        }

        internal static async Task RegistradosTask(SocketGuildUser registrador, SocketTextChannel canal)
        {
            var registradorAccount = AccountsMananger.GetAccount(registrador, registrador.Guild);

            var r = new Random();
            var emb = new EmbedBuilder();

            emb.WithAuthor(registrador.Username,
                    registrador.GetAvatarUrl() ??
                    $"https://cdn.discordapp.com/embed/avatars/{registrador.DiscriminatorValue % 5}.png",
                    registrador.GetAvatarUrl())
                .WithColor(new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)))
                //.WithThumbnailUrl(registrador.GetAvatarUrl() ??
                //                  $"https://cdn.discordapp.com/embed/avatars/{registrador.DiscriminatorValue % 5}.png")]
                .WithDescription("Perfil de registrador.")
                .AddField("<:pikachu:461345313370800129>Masculino", $"```autohotkey\nRegistrou: {registradorAccount.RegistradosMasculino} ```")
                .AddField("<:jigglypuff:461345849973407754>Feminino", $"```autohotkey\nRegistrou: {registradorAccount.RegistradosFeminino}```")
                .AddField("<:pokeballs:461345927618363392>Total",
                    $"```autohotkey\nDe membros: {registradorAccount.RegistradosMasculino + registradorAccount.RegistradosFeminino}```")
                .WithFooter(registrador.Guild.Name, registrador.Guild.IconUrl);

            await canal.SendMessageAsync("", false, emb);
        }
    }
}