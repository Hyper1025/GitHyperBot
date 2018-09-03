using System;
using Discord.Commands;
using Discord.WebSocket;
using GitHyperBot.Modules.Economia.Dependencies;

namespace GitHyperBot.Core
{
    public class Global
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static CommandService CommandService { get; set; }
        internal static Random Rng { get; set; } = new Random();
        internal static Slot Slot = new Slot(); 
    }
}