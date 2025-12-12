using System.Diagnostics;

class IngredientChecker
{
    /// <summary>
    /// Determines whether a given <paramref name="itemId"/> falls within any of the
    /// provided numeric ranges. The ranges must be sorted by their start value and
    /// must not overlap.
    /// </summary>
    /// <param name="ranges">
    /// A list of (start, end) tuples representing inclusive numeric ranges.
    /// The list is expected to be sorted and merged (no overlaps).
    /// </param>
    /// <param name="itemId">
    /// The value to test for membership within the ranges.
    /// </param>
    /// <returns>
    /// <c>true</c> if <paramref name="itemId"/> lies within any range;
    /// otherwise <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method performs a binary search over the range list, allowing
    /// membership checks in O(log n) time.
    /// </remarks>
    public static bool IsFresh(List<(long start, long end)> ranges, long itemId)
    {
        // We assume 'ranges' is already sorted and non-overlapping.
        // Perform a binary search to check whether itemId falls inside any range.

        int lo = 0;
        int hi = ranges.Count - 1;

        while (lo <= hi)
        {
            // Check the middle index value range
            int mid = (lo + hi) / 2;
            var (start, end) = ranges[mid];

            // If itemId is before this range, search the left half
            if (itemId < start)
            {
                // Set the high marker to just below the current middle position.
                hi = mid - 1;
            }
            // If itemId is after this range, search the right half
            else if (itemId > end)
            {
                // Set the low marker just above the current middle position
                lo = mid + 1;
            }
            else
            {
                // Otherwise, itemId lies between start and end (inclusive)
                return true;
            }
        }

        // No matching range found
        return false;
    }

    public static void Main()
    {
        string[] inputLines = File.ReadAllText("./input.txt")
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries);

        var completeStopwatch = Stopwatch.StartNew();

        var ranges = new List<(long start, long end)>();
        var hasMovedIntoIngredients = false;
        var items = new List<long>();

        foreach (var line in inputLines)
        {
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
                ranges.Add((start, end));
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                items.Add(long.Parse(line));
            }
        }

        // Merge the ranges so there is no overlap
        ranges = ranges
            // Step 1: Sort ranges so we can merge them in order
            .OrderBy(r => r.start)
            // Step 2: Aggregate into a new list of merged ranges
            .Aggregate(
                new List<(long start, long end)>(), // accumulator starts empty
                (merged, current) =>
                {
                    // If this is the first range, just add it
                    if (merged.Count == 0)
                    {
                        merged.Add(current);
                    }
                    else
                    {
                        // Look at the last merged range
                        var (start, end) = merged[^1];

                        // If the current range overlaps with the last merged one...
                        if (current.start <= end)
                        {
                            // ...merge them by extending the end if needed
                            merged[^1] = (start, Math.Max(end, current.end));
                        }
                        else
                        {
                            // No overlap — add as a new separate range
                            merged.Add(current);
                        }
                    }

                    // Return the accumulator for the next iteration
                    return merged;
                }
            );

        long freshItemCount = 0;
        long totalItemCount = 0;
        var stopwatch = Stopwatch.StartNew();

        foreach (var itemId in items)
        {
            if (IsFresh(ranges, itemId))
                freshItemCount++;
        }

        foreach (var (start, end) in ranges)
            totalItemCount += end - start + 1;

        stopwatch.Stop();
        completeStopwatch.Stop();
        Console.WriteLine($"Complete time : {completeStopwatch.Elapsed}");
        Console.WriteLine($"Processing time : {stopwatch.Elapsed}");
        Console.WriteLine($"There are {freshItemCount} fresh items");
        Console.WriteLine($"      and {totalItemCount} total items");
    }
}
