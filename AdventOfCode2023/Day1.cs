using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day1
    {
        public async Task Run()
        {
            var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day1.txt";
            var lines = await System.IO.File.ReadAllLinesAsync(path);
            long calibrationSum = 0;
            long correctCalibrationSum = 0;

            foreach (var line in lines)
            {
                var values = line.ToList().Where(value => Char.IsNumber(value)).Select(value => value).ToList();
                calibrationSum += int.Parse(values.First().ToString() + values.Last());
            }

            Console.WriteLine($@"The sum of calibration values is {calibrationSum}");

            foreach (var line in lines)
            {
                var fixedLine = line.Replace("one", "one1one").Replace("two", "two2two").Replace("three", "three3three")
                    .Replace("four", "four4four").Replace("five", "five5five").Replace("six", "six6six")
                    .Replace("seven", "seven7seven").Replace("eight", "eight8eight").Replace("nine", "nine9nine");
                var values = fixedLine.ToList().Where(value => Char.IsNumber(value)).Select(value => value).ToList();
                correctCalibrationSum += int.Parse(values.First().ToString() + values.Last());
            }

            Console.WriteLine($@"The sum of calibration values, corrected, is {correctCalibrationSum}");
        }
    }
}
