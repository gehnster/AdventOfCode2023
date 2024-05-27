// See https://aka.ms/new-console-template for more information

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
    case 3:
        var day3 = new Day3();
        await day3.Run();
        break;
    case 4:
        var day4 = new Day4();
        await day4.Run();
        break;
    case 5:
        var day5 = new Day5();
        await day5.Run();
        break;
    case 6:
        var day6 = new Day6();
        await day6.Run();
        break;
    case 7:
        var day7 = new Day7();
        await day7.Run();
        break;
    case 8:
        var day8 = new Day8();
        await day8.Run();
        break;
    default:
        Console.WriteLine("How did I get to default case?!");
        break;
}

Console.WriteLine("Goodbye, World!");