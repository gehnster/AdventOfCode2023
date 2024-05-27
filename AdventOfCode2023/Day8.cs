using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day8
    {
        public async Task Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day8.txt";
            var inputs = await System.IO.File.ReadAllLinesAsync(path);
            var instructions = new Queue<char>(inputs[0].ToCharArray().AsEnumerable());
            var nodes = new Dictionary<string, Node>();

            for (var i = 2; i < inputs.Length; i++)
            {
                var split = inputs[i].Split(" = ").ToList();
                var nodeName = split.First().Trim();
                var leftRight = split.Last().Replace("(", "").Replace(")", "").Split(",").ToList();
                var left = leftRight.First().Trim();
                var right = leftRight.Last().Trim();

                nodes.Add(nodeName, new Node(){ Name = nodeName, Left = left, Right = right});
            }

            var stepCount = 0L;
            var startingNode = "AAA";
            var currentNode = startingNode;

            while (currentNode != "ZZZ")
            {
                var currentInstruction = instructions.Dequeue();
                var node = nodes[currentNode];

                if (currentInstruction == 'L')
                    currentNode = node.Left;
                else
                    currentNode = node.Right;

                instructions.Enqueue(currentInstruction);
                stepCount++;
            }

            Console.WriteLine($"Total steps taken is {stepCount}");

            stepCount = 0L;
            var startingNodesPart2 = nodes.Keys.Where(x => x.EndsWith('A')).ToList();
            var currentNodesPart2 = startingNodesPart2;
            var increment = 1000000000L;

            while (!currentNodesPart2.All(x => x.EndsWith('Z')))
            {
                var currentInstruction = instructions.Dequeue();

                for (var i = 0; i < currentNodesPart2.Count; i++)
                {
                    var node = nodes[currentNodesPart2[i]];

                    if (currentInstruction == 'L')
                        currentNodesPart2[i] = node.Left;
                    else
                        currentNodesPart2[i] = node.Right;
                }

                instructions.Enqueue(currentInstruction);
                stepCount++;
                if (stepCount > increment)
                {
                    Console.WriteLine($"Increment: {stepCount}");
                    increment += 1000000000L;
                }
            }

            Console.WriteLine($"Total steps taken is {stepCount}");
            stopwatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine($"Total running time was {elapsedTime}");
        }
    }

    internal class Node
    {
        public string Name { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }
}
