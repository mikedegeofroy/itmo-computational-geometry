using System.Collections.ObjectModel;
using lab6.Interfaces;
using SharpVoronoiLib;

namespace lab6.Graph;

public class Graph : IGraph
{
    private readonly BisectorMap _edges = new();
    private readonly HashSet<Point> _sites = new();

    public void AddEdge(Edge e)
    {
        _edges.Put(e.Site1, e.Site2, e);
    }

    public Edge? GetEdgeBetweenSites(Point a, Point b)
    {
        return _edges.Get(a, b);
    }


    public void AddSite(Point newSite)
    {
        _sites.Add(newSite);
    }

    public IReadOnlyCollection<Point> GetSitePoints()
    {
        return new ReadOnlyCollection<Point>(_sites.ToList());
    }

    public override bool Equals(object? obj)
    {
        return obj is Graph g &&
               _sites.SetEquals(g._sites) &&
               _edges.Equals(g._edges);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_sites, _edges);
    }

    public IEnumerable<Edge> GetEdges()
    {
        return VoronoiPlane.TessellateOnce(
            _sites.Select(s => new VoronoiSite(s.X, s.Y)).ToList(),
            0, 0,
            600, 600,
            BorderEdgeGeneration.DoNotMakeBorderEdges
        ).Select(x => new Edge(
                new Point(x.Left.X, x.Left.Y),
                new Point(x.Right.X, x.Right.Y))
            {
                A = new Vertex(new Point(x.Start.X, x.Start.Y)),
                B = new Vertex(new Point(x.End.X, x.End.Y))
            }
        );
    }
}