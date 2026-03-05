public class Program
{
    public static void Main()
    {
        var inputLines = File.ReadAllLines("./input.txt");

        var junctionBoxes = new List<JunctionBox>();
        var circuitCombinations = new List<Circuit>();
        var union = new UnionFind<JunctionBox>();

        foreach (var line in inputLines)
        {
            var lineParts = line.Split(",");

            var x = long.Parse(lineParts[0]);
            var y = long.Parse(lineParts[1]);
            var z = long.Parse(lineParts[2]);

            var jb = new JunctionBox(x, y, z);
            junctionBoxes.Add(jb);
            union.Add(jb);
        }
        for (int i = 0; i < junctionBoxes.Count; i++)
        {
            for (int j = i + 1; j < junctionBoxes.Count; j++)
            {
                circuitCombinations.Add(new Circuit(junctionBoxes[i], junctionBoxes[j]));
            }
        }

        int connections = 0;
        foreach (var circuit in circuitCombinations.OrderBy(c => c.Distance))
        {
            if (connections >= 10)
                break;
            if (union.Connected(circuit.A, circuit.B))
                continue;
            union.Union(circuit.A, circuit.B);
            connections++;
        }
        var groups = junctionBoxes.GroupBy(jb => union.Find(jb)).ToList();
        var circuitSizes = groups.Select(g => g.Count()).ToList();

        var topThree = circuitSizes.OrderByDescending(size => size).Take(3).ToList();

        foreach (var g in groups)
        {
            Console.WriteLine($"Circuit (root {g.Key}) contains {g.Count()} boxes:");
            foreach (var jb in g)
                Console.WriteLine($"  - {jb}");
        }
        long result = topThree.Aggregate(
            1L, // Seeding with number 1 in Long format
            (runningTotal, nextNumber) => runningTotal * nextNumber
        );
        Console.WriteLine($"Final answer: {result}");
    }
}

public class JunctionBox(long x, long y, long z)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
    public long Z { get; set; } = z;

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }
}

public class Circuit
{
    public JunctionBox A { get; init; }
    public JunctionBox B { get; init; }
    public double Distance { get; init; }

    public Circuit(JunctionBox a, JunctionBox b)
    {
        A = a;
        B = b;
        Distance = GetDistance();
    }

    private double GetDistance()
    {
        double dx = A.X - B.X;
        double dy = A.Y - B.Y;
        double dz = A.Z - B.Z;

        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}

public class UnionFind<T>
{
    private readonly Dictionary<T, T> parent = new();
    private readonly Dictionary<T, int> size = new();

    public void Add(T item)
    {
        if (!parent.ContainsKey(item))
        {
            parent[item] = item; // parent of itself
            size[item] = 1;
        }
    }

    public T Find(T item)
    {
        if (parent[item].Equals(item))
            return item;

        // Path compression
        parent[item] = Find(parent[item]);
        return parent[item];
    }

    public void Union(T a, T b)
    {
        var rootA = Find(a);
        var rootB = Find(b);

        if (rootA.Equals(rootB))
            return;

        // Union by size
        if (size[rootA] < size[rootB])
        {
            parent[rootA] = rootB;
            size[rootB] += size[rootA];
        }
        else
        {
            parent[rootB] = rootA;
            size[rootA] += size[rootB];
        }
    }

    public bool Connected(T a, T b)
    {
        return Find(a).Equals(Find(b));
    }

    public int ComponentSize(T a)
    {
        return size[Find(a)];
    }
}
