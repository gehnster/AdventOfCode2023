﻿// See https://aka.ms/new-console-template for more information

using AdventOfCode2023;

Console.WriteLine("Hello, World!");

Console.WriteLine("What day do you want to run?: ");
var day = int.Parse(Console.ReadLine());

switch (day)
{
    case 1:
        var day1 = new Day1();
        await day1.Run();
        break;
    case 2:
        var day2 = new Day2();
        await day2.Run();
        break;
    default:
        Console.WriteLine("How did I get to default case?!");
        break;
}

Console.WriteLine("Goodbye, World!");