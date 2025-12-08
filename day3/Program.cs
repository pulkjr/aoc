class JoltageCalc
{
    static int MaxJoltageFromBank(string bank)
    {
        int maxJoltage = 0;

        // Get int from ascii char
        int maxLeftDigit = bank[0] - '0';

        for (int j = 1; j < bank.Length; j++)
        {
            int rightDigit = bank[j] - '0';

            // form two-digit number with best left digit so far
            int joltage = maxLeftDigit * 10 + rightDigit;
            if (joltage > maxJoltage)
                maxJoltage = joltage;

            // update maxLeftDigit if current digit is larger
            int currentDigit = bank[j] - '0';
            if (currentDigit > maxLeftDigit)
                maxLeftDigit = currentDigit;
        }

        return maxJoltage;
    }

    static void Main()
    {
        const string FILEPATH = "input.txt";
        string fileContents = File.ReadAllText(FILEPATH);
        string[] banks = fileContents
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        long total = 0;
        foreach (var bank in banks)
        {
            total += MaxJoltageFromBank(bank);
        }
        Console.WriteLine($"Total Value: {total}");
    }
}
