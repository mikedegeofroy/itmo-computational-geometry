using lab6.BeachLine;
using lab6.BeachLine.Nodes;
using lab6.Graph;
using lab6.Interfaces;

namespace lab6.Events;

public class VertexEvent : IEvent
{
    public Point Point { get; set; }
    private readonly LeafBeachNode _l;
    private readonly LeafBeachNode _c;
    private readonly LeafBeachNode _r;
    private readonly Circle _circle;

    private VertexEvent(LeafBeachNode l, LeafBeachNode c, LeafBeachNode r, Circle circle)
    {
        Point = circle.Center with { Y = circle.Center.Y - circle.Radius };
        _l = l;
        _c = c;
        _r = r;
        _circle = circle;
    }

    public static VertexEvent? Build(LeafBeachNode l, LeafBeachNode c, LeafBeachNode r)
    {
        var ap = l.Site;
        var bp = c.Site;
        var cp = r.Site;

        var convergence = (ap.Y - bp.Y) * (bp.X - cp.X) - (bp.Y - cp.Y) * (ap.X - bp.X);

        if (convergence > 0)
        {
            var circle = new Circle(ap, bp, cp);
            return circle.IsValid() ? new VertexEvent(l, c, r, circle) : null;
        }

        return null;
    }

    public void Handle(IEventQueue eventQueue, IBeachLine beachline, IGraph graph)
    {
        // Sanity checks
        if (_c.GetLeftNeighbor() != _l ||
            _c.GetRightNeighbor() != _r ||
            _l.GetRightNeighbor() != _c ||
            _r.GetLeftNeighbor() != _c)
        {
            throw new InvalidOperationException("Invalid neighbor configuration in VertexEvent.");
        }

        if (!graph.GetSitePoints().Any(p => _circle.Contains(p)))
        {
            _c.Remove();
            foreach (var e in _c.Subscribers)
                eventQueue.Remove(e);

            var v = new Vertex(_circle.Center);

            var lcEdge = graph.GetEdgeBetweenSites(_l.Site, _c.Site);
            lcEdge?.AddVertex(v);

            var rcEdge = graph.GetEdgeBetweenSites(_r.Site, _c.Site);
            rcEdge?.AddVertex(v);

            var newEdge = new Edge(_l.Site, _r.Site);
            graph.AddEdge(newEdge);
            newEdge.AddVertex(v);

            _l.AddCircleEvents(eventQueue.Enqueue, Point.Y);
            _r.AddCircleEvents(eventQueue.Enqueue, Point.Y);
        }
    }

    public int CompareTo(IEvent other)
    {
        return -Point.Y.CompareTo(other.Point.Y);
    }

    public override string ToString()
    {
        return $"VertexEvent: center = {_circle.Center}, radius = {_circle.Radius}";
    }

    private class Circle
    {
        public Point Center { get; }
        public double Radius { get; }

        public Circle(Point l, Point c, Point r)
        {
            if (l.X != c.X && c.X != r.X)
            {
                Center = ComputeCenter(l, c, r);
            }
            else if (c.X != l.X && r.X != l.X)
            {
                Center = ComputeCenter(c, l, r);
            }
            else if (c.X != r.X && l.X != r.X)
            {
                Center = ComputeCenter(c, r, l);
            }
            else
            {
                Center = new Point(double.NaN, double.NaN);
            }

            Radius = Math.Sqrt(Math.Pow(c.X - Center.X, 2) + Math.Pow(c.Y - Center.Y, 2));
        }

        private static Point ComputeCenter(Point l, Point c, Point r)
        {
            var ma = (c.Y - l.Y) / (c.X - l.X);
            var mb = (r.Y - c.Y) / (r.X - c.X);

            var x = (ma * mb * (l.Y - r.Y) + mb * (l.X + c.X) - ma * (c.X + r.X)) / (2.0 * (mb - ma));

            double y;
            if (ma != 0.0)
            {
                y = -(x - (l.X + c.X) / 2.0) / ma + (l.Y + c.Y) / 2.0;
            }
            else
            {
                y = -(x - (c.X + r.X) / 2.0) / mb + (c.Y + r.Y) / 2.0;
            }

            return new Point(x, y);
        }

        public bool IsValid()
        {
            return double.IsFinite(Radius);
        }

        public bool Contains(Point p)
        {
            return Math.Sqrt(Math.Pow(p.X - Center.X, 2) + Math.Pow(p.Y - Center.Y, 2)) < (Radius - double.Epsilon);
        }
    }
}