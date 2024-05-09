using System.Text.RegularExpressions;

namespace AdventOfCode2023;

internal partial class Day3
{
    private record Pos(int row, int colStart, int colEnd)
    {
        public bool IsAdjacent(Number number)
        {
            if (row < number.Pos.row - 1 || row > number.Pos.row + 1)
                return false;

            return colStart >= number.Pos.colStart - 1 && colStart <= number.Pos.colEnd + 1;
        }
    };

    private record Symbol(string Value, Pos Pos);
    private record Number(int Value, Pos Pos);

    public async Task Run()
    {
        var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day3.txt";
        var input = await System.IO.File.ReadAllLinesAsync(path);
        var symbols = new List<Symbol>();
        var numbers = new List<Number>();
        var numberRegex = NumbersMatch();
        var symbolRegex = SymbolsMatch();

        for (var i = 0; i < input.Length; i++)
        {
            numbers.AddRange(numberRegex.Matches(input[i]).Select(nm => new Number(Value: int.Parse(nm.Value), Pos: new Pos(i, nm.Index, nm.Index + nm.Length - 1))));

            symbols.AddRange(symbolRegex.Matches(input[i]).Select(sm => new Symbol(Value: sm.Value, Pos: new Pos(i, sm.Index, sm.Index))));
        }

        var partSum = numbers
            .Where(n => symbols.Any(s => s.Pos.IsAdjacent(n)))
            .Sum(n => n.Value);

        Console.WriteLine($@"The sum of parts is {partSum}");

        var ratioSum = symbols
            .Where(s => s.Value is "*")
            .Sum(g =>
            {
                var adjacentNumbers = numbers
                    .Where(n => g.Pos.IsAdjacent(n))
                    .ToList();
                return adjacentNumbers.Count == 2
                    ? adjacentNumbers.Aggregate(1, (acc, n) => acc * n.Value)
                    : 0;
            });

        Console.WriteLine($@"The sum of ratios is {ratioSum}");
    }

    [GeneratedRegex(@"(\d)+")]
    private static partial Regex NumbersMatch();
    [GeneratedRegex(@"[^\.\d\n]")]
    private static partial Regex SymbolsMatch();
}