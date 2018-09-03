using System;

namespace GitHyperBot.Core.Services
{
    public class ExpService
    {
        public static uint CalcularNivel(int xp)
        {
            return (uint)Math.Sqrt(xp / 10);
        }
    }
}