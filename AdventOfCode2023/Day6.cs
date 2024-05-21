using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal partial class Day6
    {
        public async Task Run()
        {
            var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day6.txt";
            var games = await System.IO.File.ReadAllLinesAsync(path);
            var numberRegex = NumbersMatch();

            var times = numberRegex.Matches(games.First().Replace("Time:", "").Trim()).Select(x => long.Parse(x.Value)).ToList();
            var distances = numberRegex.Matches(games.Last().Replace("Distance:", "").Trim()).Select(x => long.Parse(x.Value)).ToList();

            var waysToBeatBestDistance = 1;
            for (var i = 0; i < times.Count; i++)
            {
                var maxDistance = distances[i];
                var numberOfPotentialWins = 0;

                var timeHeld = 0L;
                for (var totalTime = times[i]; totalTime >= 0; totalTime--)
                {
                    if(timeHeld * totalTime > maxDistance)
                        numberOfPotentialWins++;

                    timeHeld++;
                }

                waysToBeatBestDistance *= numberOfPotentialWins;
            }

            Console.WriteLine($"Multiplied Wins number is {waysToBeatBestDistance}");

            var time = long.Parse(games.First().Replace("Time:", "").Replace(" ", ""));
            var distance = long.Parse(games.Last().Replace("Distance:", "").Replace(" ", ""));

            waysToBeatBestDistance = 1;
            var numberOfPotentialWinsPart2 = 0;

            var timeHeldPart2 = 0L;
            for (var totalTime = time; totalTime >= 0; totalTime--)
            {
                if (timeHeldPart2 * totalTime > distance)
                    numberOfPotentialWinsPart2++;

                timeHeldPart2++;
            }

            waysToBeatBestDistance *= numberOfPotentialWinsPart2;

            Console.WriteLine($"Part 2 Multiplied Wins number is {waysToBeatBestDistance}");
        }

        [GeneratedRegex(@"(\d)+")]
        private static partial Regex NumbersMatch();
    }
}
