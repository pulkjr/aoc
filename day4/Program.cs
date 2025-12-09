using System.Drawing;

class PrintingPuzzle
{
    public static readonly Point[] Points =
    [
        new Point(-1, 0), // Left
        new Point(-1, 1), // Left Up
        new Point(0, 1), // Up
        new Point(1, 1), // Up right
        new Point(1, 0), // Right
        new Point(1, -1), // Down right
        new Point(0, -1), // Down
        new Point(-1, -1), // Down Left
    ];

    public static Point GetOffset(Point pos, Point offset)
    {
        return new Point(pos.X + offset.X, pos.Y + offset.Y);
    }

    public static bool RollIsAccessible(string[] puzzleMap, Point pos)
    {
        if (puzzleMap[pos.X][pos.Y] != '@')
        {
            return false;
        }
        if (pos.X == 0 && pos.Y == 0)
        {
            return true;
        }
        var rollCount = 0;
        foreach (var point in Points.AsEnumerable())
        {
            Point newPos = GetOffset(pos, point);
            // Console.WriteLine($"Start: {pos} New Position = {newPos}");
            if (
                newPos.X < 0
                || newPos.Y < 0
                || newPos.X >= puzzleMap.Length
                || newPos.Y >= puzzleMap[newPos.X].Length
            )
            {
                continue;
            }
            if (puzzleMap[newPos.X][newPos.Y] == '@')
            {
                rollCount++;
            }
        }

        // Console.WriteLine($" Final Count = {rollCount}");
        return rollCount < 4;
    }

    public static void Main()
    {
        string[] puzzle = File.ReadAllText("./input.txt")
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var finalCountOfRolls = 0;

        Console.WriteLine($"Rows: {puzzle.Length}");
        Console.WriteLine($"Columns: {puzzle[0].Length}");
        for (var row = 0; row < puzzle.Length; row++)
        {
            for (var col = 0; col < puzzle[row].Length; col++)
            {
                var pos = new Point(row, col);

                if (RollIsAccessible(puzzle, pos))
                {
                    finalCountOfRolls++;
                }
            }
        }
        Console.WriteLine($"The final count of rolls inaccessible is: {finalCountOfRolls}");
    }
}
