using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // If no file path is given, default to "input.txt"
        string filePath = args.Length > 0 ? args[0] : "input.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Input file not found: {filePath}");
            return;
        }

        // Dial starts at 50
        int position = 50;
        int zeroCount = 0;

        foreach (var line in File.ReadLines(filePath))
        {
            string instruction = line.Trim();
            if (string.IsNullOrEmpty(instruction))
                continue;

            char direction = instruction[0];
            int distance = int.Parse(instruction[1..]);

            if (direction == 'L')
            {
                position = (position - distance) % 100;
                if (position < 0)
                    position += 100; // wrap around
            }
            else if (direction == 'R')
            {
                position = (position + distance) % 100;
            }
            else
            {
                Console.WriteLine($"Invalid instruction: {instruction}");
                continue;
            }

            if (position == 0)
            {
                zeroCount++;
            }
        }

        Console.WriteLine($"Password: {zeroCount}");
    }
}
