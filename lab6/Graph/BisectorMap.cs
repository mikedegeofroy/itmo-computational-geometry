namespace lab6.Graph;

internal class BisectorMap
{
    private readonly Dictionary<Bisector, Edge> _data = new();

    public void Put(Point a, Point b, Edge e)
    {
        var key = new Bisector(a, b);
        if (!_data.TryAdd(key, e))
            throw new ArgumentException("Bisector already exists.");
    }

    public Edge? Get(Point a, Point b)
    {
        var key = new Bisector(a, b);
        return _data.GetValueOrDefault(key);
    }

    public ICollection<Edge> Values()
    {
        return _data.Values;
    }

    public IEnumerable<Edge> GetEdges()
    {
        return _data.Values.AsEnumerable();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BisectorMap other)
            return false;

        return _data.Count == other._data.Count &&
               _data.Keys.All(other._data.ContainsKey);
    }

    public override int GetHashCode()
    {
        return _data.Aggregate(0, (hash, kv) => hash ^ kv.Key.GetHashCode());
    }

    private class Bisector : IEquatable<Bisector>
    {
        private readonly Point _a;
        private readonly Point _b;

        public Bisector(Point a, Point b)
        {
            _a = a ?? throw new ArgumentNullException(nameof(a));
            _b = b ?? throw new ArgumentNullException(nameof(b));
        }

        public bool Equals(Bisector? other)
        {
            if (other == null) return false;

            // symmetric equality: (a,b) == (b,a)
            return (_a.Equals(other._a) && _b.Equals(other._b)) ||
                   (_a.Equals(other._b) && _b.Equals(other._a));
        }

        public override bool Equals(object? obj)
        {
            return obj is Bisector other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _a.GetHashCode() + _b.GetHashCode();
        }
    }
}