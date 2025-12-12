public class MovieFloor
{
    public static void Main()
    {
        var lines = File.ReadAllLines("./input.txt");
        var points = new List<(long, long)>();

        long largestRectangleSize = 0L;
        foreach (var line in lines)
        {
            var positions = line.Split(",");
            var updatedPositions = (long.Parse(positions[0]), long.Parse(positions[1]));
            points.Add(updatedPositions);
        }
        for (var i = 0; i < points.Count; i++)
        {
            for (var j = i; j < points.Count; j++)
            {
                var A = points[i];
                var B = points[j];
                var sizeOfRectangle =
                    (Math.Abs(A.Item1 - B.Item1) + 1) * (Math.Abs(A.Item2 - B.Item2) + 1);

                if (largestRectangleSize < sizeOfRectangle)
                    largestRectangleSize = sizeOfRectangle;
            }
        }
        Console.WriteLine($"Largest Rectangle is {largestRectangleSize}");
    }
}
