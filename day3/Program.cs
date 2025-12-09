class JoltageCalc
{
    static string MaxJoltage(string digits, int length)
    {
        int toRemove = digits.Length - length;
        var stack = new Stack<char>();

        Console.WriteLine($"Input digits: {digits}, need length {length}, can remove {toRemove}");

        foreach (char d in digits)
        {
            Console.WriteLine($"\nConsidering digit: {d}");
            while (stack.Count > 0 && toRemove > 0 && stack.Peek() < d)
            {
                char removed = stack.Pop();
                toRemove--;
                Console.WriteLine(
                    $"  Popped {removed} (smaller than {d}), remaining removals: {toRemove}"
                );
            }
            stack.Push(d);
            Console.WriteLine($"  Pushed {d}, stack now: [{string.Join("", stack.Reverse())}]");
        }

        // If we still need to remove, drop from the end
        while (toRemove > 0)
        {
            char removed = stack.Pop();
            toRemove--;
            Console.WriteLine($"Removing extra {removed} from end, remaining removals: {toRemove}");
        }

        // Build result in correct order
        var result = new char[length];
        for (int i = length - 1; i >= 0; i--)
        {
            result[i] = stack.Pop();
        }

        return new string(result);
    }

    static void Main()
    {
        const string FILEPATH = "input.txt";
        string fileContents = File.ReadAllText(FILEPATH);
        string[] banks = fileContents
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        decimal total = 0;
        foreach (var bank in banks)
        {
            var finalNum = MaxJoltage(bank, 12);
            total += decimal.Parse(finalNum);
        }
        Console.WriteLine($"Total Value: {total}");
    }
}
