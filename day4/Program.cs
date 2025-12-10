using System.Diagnostics;
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

    private static int Index(int row, int col, int columnCount) => row * columnCount + col;

    public static Point GetOffset(Point pos, Point offset)
    {
        return new Point(pos.X + offset.X, pos.Y + offset.Y);
    }

    public static bool RollIsAccessible(
        Span<char> puzzleMap,
        Point pos,
        int rowCount,
        int columnCount
    )
    {
        if (puzzleMap[Index(pos.X, pos.Y, columnCount)] != '@')
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

            if (newPos.X < 0 || newPos.Y < 0 || newPos.X >= rowCount || newPos.Y >= columnCount)
            {
                continue;
            }
            if (puzzleMap[Index(newPos.X, newPos.Y, columnCount)] == '@')
            {
                rollCount++;
            }
        }

        // Console.WriteLine($" Final Count = {rollCount}");
        return rollCount < 4;
    }

    public static void PrintGrid(Span<char> puzzle, int rows, int cols)
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Console.Write(puzzle[Index(r, c, cols)]);
            }
            Console.WriteLine();
        }
    }

    public static void Main()
    {
        var stopwatch = Stopwatch.StartNew();
        string[] puzzleLines = File.ReadAllText("./input.txt")
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        int rowCount = puzzleLines.Length;
        int columnCount = puzzleLines[0].Length;

        // Flatten puzzle into one contiguous buffer
        char[] buffer = new char[rowCount * columnCount];
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                buffer[Index(r, c, columnCount)] = puzzleLines[r][c];
            }
        }
        Span<char> puzzle = buffer.AsSpan();
        stopwatch.Stop();
        Console.WriteLine($"Setup took: {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Restart();

        var finalCountOfRolls = 0;

        bool changed = true;

        while (changed)
        {
            changed = false;
            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < columnCount; col++)
                {
                    var pos = new Point(row, col);

                    if (RollIsAccessible(puzzle, pos, rowCount, columnCount))
                    {
                        puzzle[Index(row, col, columnCount)] = '.';
                        changed = true;
                        finalCountOfRolls++;
                    }
                }
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Puzzle Processing took: {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"The final count of rolls inaccessible is: {finalCountOfRolls}");
        PrintGrid(puzzle, rowCount, columnCount);
    }
}
