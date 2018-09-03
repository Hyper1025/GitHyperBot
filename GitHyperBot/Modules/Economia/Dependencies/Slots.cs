//Fonte : https://github.com/petrspelos/Community-Discord-BOT/blob/647f20f35bf96d44cecb48a0da2f1b9655ea1b80/CommunityBot/Features/Economy/Slots.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using GitHyperBot.Core;

namespace GitHyperBot.Modules.Economia.Dependencies
{
    /// <summary>
    /// Slots looks something similar like this:
    /// 🍇🍓🍇
    /// 🍔🍒🍍 
    /// 💯🍓🍍
    /// A Slot machine consists of 3 Cylinders that individually can rotate vertically
    /// Each Cylinder has n SlotPieces (Emojis + some extra information)
    /// </summary>
    public class Slot
    {
        // This is really something that shouldn't be hardcoded :D but oh well... works for now :P has to be tweaked for balance of win/loss ration tho
        public static readonly List<SlotPiece> PossibleSlotPieces = new List<SlotPiece>
        {
          // new SlotPiece("emojiString", minSpawnCount, spawnRate, payoutRate); 
             new SlotPiece(":100:",         1, 1, 50  ),
             new SlotPiece(":candy:",       1, 2, 10  ),
             new SlotPiece(":strawberry:",  2, 2, 5   ),
             new SlotPiece(":pineapple: ",  3, 2, 3   ),
             new SlotPiece(":grapes:",      3, 2, 1   ),
             new SlotPiece(":cherries:",    3, 2, 0.5 )
        };
        public readonly List<Cylinder> Cylinders = new List<Cylinder>();

        // Will be the sum of the spawnRates of all possible SlotPieces
        private static int _maxRandom;

        // The amount of pieces per cylinder is adjustable but will always be at least the sum of the minSpawnCount of all possible SlotPieces
        public Slot(int amountOfPices = 0)
        {
            _maxRandom = 0;
            foreach (var piece in PossibleSlotPieces)
            {
                _maxRandom += piece.Spawnrate;
            }
            Cylinders.Add(new Cylinder(amountOfPices));
            Cylinders.Add(new Cylinder(amountOfPices));
            Cylinders.Add(new Cylinder(amountOfPices));
        }

        public class Cylinder
        {
            public List<SlotPiece> SlotPieces = new List<SlotPiece>();
            // We are not really spinning anything - we just move a pointer and have everything offsetted by it
            public int Pointer;
            public Cylinder(int size)
            {
                // Add all the pieces minSpawnCount times
                foreach (var piece in PossibleSlotPieces)
                {
                    for (var i = piece.MinSpawnCount - 1; i >= 0; i--)
                    {
                        SlotPieces.Add(piece);
                        size--;
                    }
                }
                // If more pieces are requested, pick a random pice weighted by their spawnrate and add it
                for (var i = size; i > 0; i--)
                {
                    var rand = Global.Rng.Next(_maxRandom);
                    foreach (var piece in PossibleSlotPieces)
                    {
                        rand -= piece.Spawnrate;
                        if (rand > 0) continue;
                        SlotPieces.Add(piece);
                        break;
                    }
                }
                // Shuffle the pieces
                SlotPieces = SlotPieces.OrderBy((item) => Global.Rng.Next()).ToList();
            }
        }

        public class SlotPiece
        {
            public string Emoji;
            public int MinSpawnCount;
            public int Spawnrate;
            public double Payout;

            public SlotPiece(string emoji, int minSpawnCount, int spawnrate, double payout)
            {
                Emoji = emoji;
                MinSpawnCount = minSpawnCount;
                Spawnrate = spawnrate;
                Payout = payout;
            }
        };

        // Returns the amount of Miunies you win with the current pointers of the Cylinders if you bet <amount> of Miunies
        // And a flavour string depending on how much you lose/win
        public Tuple<uint, string, Color> GetPayoutAndFlavourText(uint amount)
        {
            double payoutModifier = 0;

            /*
             * Emoji coordinates (row, column):
             *  0, 0 | 0, 1 | 0, 2
             *  1, 0 | 1, 1 | 1, 2
             *  2, 0 | 2, 1 | 2, 2
             */

            for (var i = 0; i < 3; i++)
            {
                // Check columns
                payoutModifier += CheckPayoutForCoordinates(0, i, 1, i, 2, i);
                // Check rows
                payoutModifier += CheckPayoutForCoordinates(i, 0, i, 1, i, 2);
            }
            // Diagonal top left to bottom right
            payoutModifier += CheckPayoutForCoordinates(0, 0, 1, 1, 2, 2);
            // Diagonal bottom left to top right
            payoutModifier += CheckPayoutForCoordinates(2, 0, 1, 1, 0, 2);

            var moneyGain = (uint)(amount * payoutModifier);
            var flavourText = "Você jogou e ";
            Color cor;

            if (moneyGain > amount)
            {
                flavourText += $"lucrou **{moneyGain} golds** BOA!";
                cor = Color.Green;
            }
            else if (moneyGain == amount)
            {
                flavourText += "você ganhou seu dinheiro de volta... Bom, poderia ser pior...";
                cor = Color.Gold;
            }
            else if (moneyGain > 0)
            {
                flavourText += $"ganhou algo de volta... pegue esses {moneyGain} golds!";
                cor = Color.Blue;
            }
            else
            {
                flavourText += "perdeu tudo!";
                cor = Color.Red;
            }

            return Tuple.Create(moneyGain, flavourText, cor);
        }

        // Check if the given set of three coordinates if all are the same emoji - if so return the payout ratio of that emoji
        private double CheckPayoutForCoordinates(int aRow, int aColumn, int bRow, int bColumn, int cRow, int cColumn)
        {
            var count = Cylinders[0].SlotPieces.Count;
            var first = Cylinders[aColumn].SlotPieces[(Cylinders[aColumn].Pointer + aRow) % count];
            var second = Cylinders[bColumn].SlotPieces[(Cylinders[bColumn].Pointer + bRow) % count];
            var third = Cylinders[cColumn].SlotPieces[(Cylinders[cColumn].Pointer + cRow) % count];
            if (first.Emoji == second.Emoji && second.Emoji == third.Emoji)
                return first.Payout;
            return 0;
        }

        /// <summary>
        /// Returns a List containing 3 (emoji) strings to show Pieces of the slot machine
        /// </summary>
        /// <param name="showAll">If true the list will contain all Pieces if not only the 9 to display for playing</param>
        /// <returns></returns>
        public List<string> GetCylinderEmojis(bool showAll = false)
        {
            var response = new List<string>();
            var piceCount = Cylinders[0].SlotPieces.Count;
            var loopMax = showAll ? piceCount : 3;
            for (var j = 0; j < loopMax; j++)
            {
                var cylinderString = "";
                for (var i = 0; i < 3; i++)
                {
                    cylinderString += Cylinders[i].SlotPieces[(Cylinders[i].Pointer + j) % piceCount].Emoji;
                }
                response.Add(cylinderString);
            }
            return response;
        }

        public string Spin()
        {
            var count = Cylinders[0].SlotPieces.Count;
            Cylinders[0].Pointer = Global.Rng.Next(count);
            Cylinders[1].Pointer = Global.Rng.Next(count);
            Cylinders[2].Pointer = Global.Rng.Next(count);
            return string.Join("\n", GetCylinderEmojis());
        }
    }
}