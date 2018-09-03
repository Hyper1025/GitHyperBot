using System;
using System.Threading.Tasks;
using System.Timers;

namespace GitHyperBot.Core.Services
{
    public class ReadyServices
    {
        private static Timer _loopTimer;

        internal static Task StartTimer()
        {
            _loopTimer = new Timer()
            {
                Interval = 3600000,
                AutoReset = true,
                Enabled = true
            };
            _loopTimer.Elapsed += _loopTimer_Elapsed;

            return Task.CompletedTask;
        }

        private static void _loopTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
        }
    }
}