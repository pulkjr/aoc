class InvalidIdChecker
{
    static bool IsInvalid(long id)
    {
        string s = id.ToString();
        string doubled = s + s;
        string trimmed = doubled.Substring(1, doubled.Length - 2);
        return trimmed.Contains(s);
    }

    static long SumInvalidIds(long start, long end)
    {
        long sum = 0;
        for (long i = start; i <= end; i++)
        {
            if (IsInvalid(i))
                sum += i;
        }
        return sum;
    }

    static void Main()
    {
        const string FILEPATH = "input.txt";
        string fileContents = File.ReadAllText(FILEPATH);
        string[] ranges = fileContents.Split(",");
        long total = 0;

        foreach (string numberRange in ranges)
        {
            string[] twoNums = numberRange.Split("-");
            long lowerRangeLong = long.Parse(twoNums[0]);
            long upperRangeLong = long.Parse(twoNums[1]);
            total += SumInvalidIds(lowerRangeLong, upperRangeLong);
        }

        Console.WriteLine($"Total invalid IDs sum: {total}");
    }
}
