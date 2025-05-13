namespace lab6.Models;

public class Edge
{
    public Point? Start { get; init; }
    public Point? End { get; set; }
    public Point Direction { get; init; }

    public Point LeftSite { get; init; }
    public Point RightSite { get; init; }

    public Edge(Point start, Point direction, Point leftSite, Point rightSite)
    {
        Start = start;
        Direction = direction;
        LeftSite = leftSite;
        RightSite = rightSite;
    }

    public Point? GetPointAt(float t)
    {
        if (Start is null) return null;

        return new Point(
            Start.X + Direction.X * t,
            Start.Y + Direction.Y * t
        );
    }

    public static Edge CreateBisector(Point siteA, Point siteB, Point start)
    {
        var dx = siteB.X - siteA.X;
        var dy = siteB.Y - siteA.Y;
        var direction = new Point(dy, -dx);

        return new Edge(start, direction, siteA, siteB);
    }
}