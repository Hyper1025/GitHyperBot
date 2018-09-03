using System;
using System.Linq;
using Discord.WebSocket;

namespace GitHyperBot.Modules.Admin.Dependencies
{
    public class GetRoleIdService
    {
        internal static ulong GetRole(SocketGuild guild, string nomeDaRole)
        {
            var roleRetornada = guild.Roles.FirstOrDefault(x => string.Equals(x.Name, nomeDaRole, StringComparison.CurrentCultureIgnoreCase));

            return roleRetornada?.Id ?? (ulong) 0;
        }
    }
}