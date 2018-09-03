using System.Collections.Generic;

namespace GitHyperBot.Core.Services
{
    public class ConversorDeNumerosService
    {
        private static readonly Dictionary<char, string> NumberEmojiMap = new Dictionary<char, string>
        {
            {'0', ":zero:"},
            {'1', ":one:"},
            {'2', ":two:"},
            {'3', ":three:"},
            {'4', ":four:"},
            {'5',":five:"},
            {'6',":six:"},
            {'7'," :seven:"},
            {'8',":eight:"},
            {'9',":nine:" }
        };

        public static string NumeroParaEmoji(string numero)
        {
            var output = string.Empty;

            foreach (var item in numero)
            {
                output += NumberEmojiMap[item];
            }

            return output;
        }
    }
}