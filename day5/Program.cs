using System.Diagnostics;

class IngredientChecker
{
    public static bool IsFresh(Dictionary<long, long> range, long itemId)
    {
        foreach (long start in range.Keys.AsEnumerable())
        {
            if (itemId == start || itemId == range[start])
                return true;

            if (itemId > start && itemId < range[start])
                return true;
        }
        return false;
    }

    public static void Main()
    {
        string[] inputLines = File.ReadAllText("./input.txt")
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries);

        var range = new Dictionary<long, long>();
        var hasMovedIntoIngredients = false;
        long freshItemCount = 0;
        var stopwatch = Stopwatch.StartNew();
        foreach (var line in inputLines)
        {
            Console.WriteLine(line);
            if (!hasMovedIntoIngredients && string.IsNullOrWhiteSpace(line))
            {
                hasMovedIntoIngredients = true;
                continue;
            }
            if (!hasMovedIntoIngredients)
            {
                var values = line.Split('-');
                var start = long.Parse(values[0]);
                var end = long.Parse(values[1]);
                if (!range.TryAdd(start, end))
                {
                    if (range[start] <= end)
                    {
                        range[start] = end;
                    }
                }
                continue;
            }
            if (string.IsNullOrWhiteSpace(line))
                continue;
            long itemId = long.Parse(line);
            if (IsFresh(range, itemId))
                freshItemCount++;
        }
        stopwatch.Stop();
        Console.WriteLine($"Processing time : {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"There are {freshItemCount} fresh items!");
    }
}
