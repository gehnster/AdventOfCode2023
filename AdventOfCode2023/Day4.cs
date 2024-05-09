using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal partial class Day4
    {
        public async Task Run()
        {
            var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day4.txt";
            var games = await System.IO.File.ReadAllLinesAsync(path);
            var totalPoints = 0;
            var numberRegex = NumbersMatch();
            var cardsPlayedTotal = 0;
            var cardCopyStack = new Stack<int>();
            var wonCardsDictionary = new Dictionary<int, int>();

            var cardNumber = 1;
            foreach (var game in games)
            {
                var parts = game.Split(":").Last().Split("|");
                var winningNumbers = numberRegex.Matches(parts.First().Trim()).Select(x => int.Parse(x.Value));
                var yourNumbers = numberRegex.Matches(parts.Last().Trim()).Select(x => int.Parse(x.Value));

                var totalWins = yourNumbers.Where(x => winningNumbers.Contains(x)).Select(x => x);
                var totalWinsCount = totalWins.Count();
                var gameTotal = totalWinsCount > 0 ? (int)Math.Pow(2, totalWinsCount - 1) : 0;
                totalPoints += gameTotal;
                cardsPlayedTotal++;

                if (totalWinsCount > 0)
                {
                    for (var i = cardNumber + 1; i < cardNumber+1 + totalWinsCount; i++)
                    {
                        cardCopyStack.Push(i);
                    }
                }

                wonCardsDictionary.Add(cardNumber, totalWinsCount);
                cardNumber++;
            }

            Console.WriteLine($@"Your total points is {totalPoints}");

            while (cardCopyStack.Any())
            {
                cardsPlayedTotal++;
                var cardCopy = cardCopyStack.Pop();
                var totalWinsCount = wonCardsDictionary[cardCopy];

                if (totalWinsCount > 0)
                {
                    for (var i = cardCopy + 1; i < cardCopy + 1 + totalWinsCount; i++)
                    {
                        cardCopyStack.Push(i);
                    }
                }
            }

            Console.WriteLine($@"Your total cards won is {cardsPlayedTotal}");
        }

        [GeneratedRegex(@"(\d)+")]
        private static partial Regex NumbersMatch();
    }
}
