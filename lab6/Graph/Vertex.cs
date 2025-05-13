namespace lab6.Graph;

public class Vertex(Point location)
{
    public Point Location { get; } = location ?? throw new ArgumentNullException(nameof(location));

    public override string ToString()
    {
        return Location.ToString();
    }

    public override bool Equals(object? obj)
    {
        return obj is Vertex other && Location.Equals(other.Location);
    }

    public override int GetHashCode()
    {
        return Location.GetHashCode();
    }
}