using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day2
    {
        public async Task Run()
        {
            var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day2.txt";
            var games = await System.IO.File.ReadAllLinesAsync(path);
            var minNumber = 12;
            var maxRed = minNumber;
            var maxGreen = minNumber+1;
            var maxBlue = minNumber+2;
            var idSum = 0;

            foreach (var game in games)
            {
                var badGame = false;
                var newLines = game.Replace("Game ", "").Split(":").ToList();
                var hands = newLines.Last().Split(";").ToList();
                foreach (var colorNumber in hands.Select(hand => hand.Split(",").ToList()))
                {
                    foreach (var color in colorNumber)
                    {
                        var colorSplit = color.Trim().Split(" ");
                        var number = int.Parse(colorSplit.First());
                        if (number > maxRed && colorSplit.Last() == "red")
                        {
                            badGame = true;
                            break;
                        }
                        else if (number > maxGreen && colorSplit.Last() == "green")
                        {
                            badGame = true;
                            break;
                        }
                        else if (number > maxBlue && colorSplit.Last() == "blue")
                        {
                            badGame = true;
                            break;
                        }
                    }

                    if (badGame)
                    {
                        break;
                    }
                }

                if (!badGame)
                {
                    idSum += int.Parse(newLines.First());
                }
            }

            Console.WriteLine($@"The sum of ids is {idSum}");

            var powerSum = 0;

            foreach (var game in games)
            {
                int? minRed = null;
                int? minGreen = null;
                int? minBlue = null;
                var hands = game.Split(":").ToList().Last().Split(";").ToList();
                foreach (var colorNumber in hands.Select(hand => hand.Split(",").ToList()))
                {
                    foreach (var color in colorNumber)
                    {
                        var colorSplit = color.Trim().Split(" ");
                        var number = int.Parse(colorSplit.First());
                        switch (colorSplit.Last())
                        {
                            case "red":
                                minRed = minRed.HasValue ? number > minRed ? number : minRed : number;
                                break;
                            case "green":
                                minGreen = minGreen.HasValue ? number > minGreen ? number : minGreen : number;
                                break;
                            case "blue":
                                minBlue = minBlue.HasValue ? number > minBlue ? number : minBlue : number;
                                break;
                        }
                    }
                }

                minRed ??= 1;
                minGreen ??= 1;
                minBlue ??= 1;
                powerSum += (minRed.Value * minGreen.Value * minBlue.Value);
            }

            Console.WriteLine($@"The sum of powers is {powerSum}");
        }
    }
}
