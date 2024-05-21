using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day5
    {
        public async Task Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day5.txt";
            var games = await System.IO.File.ReadAllLinesAsync(path);

            var seeds = new List<long>();
            var longSeeds = new List<SeedRange>();

            var soilMapBuild = false;
            var fertilizerMapBuild = false;
            var waterMapBuild = false;
            var lightMapBuild = false;
            var tempMapBuild = false;
            var humidityMapBuild = false;
            var locationMapBuild = false;

            var soilMap = new List<Range>();
            var waterMap = new List<Range>();
            var lightMap = new List<Range>();
            var tempMap = new List<Range>();
            var fertilizerMap = new List<Range>();
            var humidityMap = new List<Range>();
            var locationMap = new List<Range>();

            foreach (var game in games)
            {
                if (game.StartsWith("seeds:"))
                {
                    seeds = await ParseSeeds(game);
                    longSeeds = await ParseLongSeeds(game);
                }
                else if (game.StartsWith("seed-to-soil map:"))
                {
                    soilMapBuild = true;
                    continue;
                }
                else if (game.StartsWith("soil-to-fertilizer map:"))
                {
                    soilMapBuild = false;
                    fertilizerMapBuild = true;
                    continue;
                }
                else if (game.StartsWith("fertilizer-to-water map:"))
                {
                    fertilizerMapBuild = false;
                    waterMapBuild = true;
                    continue;
                }
                else if (game.StartsWith("water-to-light map:"))
                {
                    waterMapBuild = false;
                    lightMapBuild = true;
                    continue;
                }
                else if (game.StartsWith("light-to-temperature map:"))
                {
                    lightMapBuild = false;
                    tempMapBuild = true;
                    continue;
                }
                else if (game.StartsWith("temperature-to-humidity map:"))
                {
                    tempMapBuild = false;
                    humidityMapBuild = true;
                    continue;
                }
                else if (game.StartsWith("humidity-to-location map:"))
                {
                    humidityMapBuild = false;
                    locationMapBuild = true;
                    continue;
                }
                else if (game == String.Empty)
                {
                    continue;
                }
                else
                {
                    if (soilMapBuild)
                    {
                        var parsedNumbers = game.Trim().Split(" ").Select(x => long.Parse(x)).ToList();
                        soilMap.Add(new Range() { DestinationStart = parsedNumbers[0], SourceStart = parsedNumbers[1], Length = parsedNumbers[2] });
                    }
                    else if (fertilizerMapBuild)
                    {
                        var parsedNumbers = game.Trim().Split(" ").Select(x => long.Parse(x)).ToList();
                        fertilizerMap.Add(new Range() { DestinationStart = parsedNumbers[0], SourceStart = parsedNumbers[1], Length = parsedNumbers[2] });
                    }
                    else if (waterMapBuild)
                    {
                        var parsedNumbers = game.Trim().Split(" ").Select(x => long.Parse(x)).ToList();
                        waterMap.Add(new Range() { DestinationStart = parsedNumbers[0], SourceStart = parsedNumbers[1], Length = parsedNumbers[2] });
                    }
                    else if (lightMapBuild)
                    {
                        var parsedNumbers = game.Trim().Split(" ").Select(x => long.Parse(x)).ToList();
                        lightMap.Add(new Range() { DestinationStart = parsedNumbers[0], SourceStart = parsedNumbers[1], Length = parsedNumbers[2] });
                    }
                    else if (tempMapBuild)
                    {
                        var parsedNumbers = game.Trim().Split(" ").Select(x => long.Parse(x)).ToList();
                        tempMap.Add(new Range() { DestinationStart = parsedNumbers[0], SourceStart = parsedNumbers[1], Length = parsedNumbers[2] });
                    }
                    else if (humidityMapBuild)
                    {
                        var parsedNumbers = game.Trim().Split(" ").Select(x => long.Parse(x)).ToList();
                        humidityMap.Add(new Range() { DestinationStart = parsedNumbers[0], SourceStart = parsedNumbers[1], Length = parsedNumbers[2] });
                    }
                    else if (locationMapBuild)
                    {
                        var parsedNumbers = game.Trim().Split(" ").Select(x => long.Parse(x)).ToList();
                        locationMap.Add(new Range() { DestinationStart = parsedNumbers[0], SourceStart = parsedNumbers[1], Length = parsedNumbers[2] });
                    }
                }
            }

            await LowestLocationNumber(seeds, soilMap, fertilizerMap, waterMap, lightMap, tempMap, humidityMap, locationMap);
            LowestLocationNumber(longSeeds, soilMap, fertilizerMap, waterMap, lightMap, tempMap, humidityMap, locationMap);
            stopwatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine($"Total running time was {elapsedTime}");
        }

        private async Task LowestLocationNumber(List<long> seeds, List<Range> soilMap, List<Range> fertilizerMap,
            List<Range> waterMap, List<Range> lightMap, List<Range> tempMap, List<Range> humidityMap,
            List<Range> locationMap)
        {
            var lowestLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                var soilNumber = FindValue(seed, soilMap);
                var fertilizerNumber = FindValue(soilNumber, fertilizerMap);
                var waterNumber = FindValue(fertilizerNumber, waterMap);
                var lightNumber = FindValue(waterNumber, lightMap);
                var tempNumber = FindValue(lightNumber, tempMap);
                var humidityNumber = FindValue(tempNumber, humidityMap);
                var locationNumber = FindValue(humidityNumber, locationMap);

                if (locationNumber < lowestLocation)
                    lowestLocation = locationNumber;
            }

            Console.WriteLine($"The lowest location value is: {lowestLocation}");
        }

        private void LowestLocationNumber(List<SeedRange> seeds, List<Range> soilMap, List<Range> fertilizerMap,
            List<Range> waterMap, List<Range> lightMap, List<Range> tempMap, List<Range> humidityMap,
            List<Range> locationMap)
        {
            var lowestLocation = long.MaxValue;

            foreach (var seedRange in seeds)
            {
                for (var i = seedRange.Start; i < seedRange.Start+seedRange.Length; i++)
                {
                    var locationNumber = FindValue(FindValue(FindValue(FindValue(FindValue(FindValue(FindValue(i, soilMap), fertilizerMap), waterMap), lightMap), tempMap), humidityMap), locationMap);

                    if (locationNumber < lowestLocation)
                        lowestLocation = locationNumber;
                }
            }

            Console.WriteLine($"The lowest location value for seed range is: {lowestLocation}");
        }

        private long FindValue(long number, List<Range> map)
        {
            foreach (var range in map)
            {
                if (range.SourceStart <= number && range.SourceStart + range.Length >= number)
                {
                    return range.DestinationStart + (number - range.SourceStart);
                }
            }

            return number;
        }

        private async Task<List<long>> ParseSeeds(string line)
        {
            return line.Replace("seeds:", "").Trim().Split(" ").Select(x => long.Parse(x)).ToList();
        }

        private async Task<List<SeedRange>> ParseLongSeeds(string line)
        {
            var seedList = line.Replace("seeds:", "").Trim().Split(" ").Select(x => long.Parse(x)).ToList();
            var seeds = new List<SeedRange>();

            for (var i = 0; i < seedList.Count; i+=2)
            {
                seeds.Add(new SeedRange() { Start = seedList[i], Length = seedList[i+1] });
            }

            return seeds;
        }
    }

    internal class Range
    {
        public long SourceStart { get; set; }
        public long DestinationStart { get; set; }
        public long Length { get; set; }
    }

    internal class SeedRange
    {
        public long Start { get; set; }
        public long Length { get; set; }
    }
}
