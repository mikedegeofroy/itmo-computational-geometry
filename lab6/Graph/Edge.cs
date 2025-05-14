namespace lab6.Graph;

public class Edge
{
    public Point? Site1 { get; }
    public Point? Site2 { get; }

    public Vertex? A { get; set; }
    public Vertex? B { get; set; }

    public Edge()
    {
    }

    public Edge(Point site1, Point site2)
    {
        Site1 = site1 ?? throw new ArgumentNullException(nameof(site1));
        Site2 = site2 ?? throw new ArgumentNullException(nameof(site2));
    }

    public void AddVertex(Vertex v)
    {
        if (A == null)
        {
            A = v;
        }
        else if (B == null)
        {
            B = v;
        }
        else
        {
            throw new InvalidOperationException("Trying to set a third vertex on an edge.");
        }
    }

    public override string ToString()
    {
        return $"Edge({A}, {B})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Edge other)
            return false;

        return Site1.Equals(other.Site1)
               && Site2.Equals(other.Site2)
               && Equals(A, other.A)
               && Equals(B, other.B);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Site1, Site2, A, B);
    }
}