using System.Collections.ObjectModel;
using lab6.Interfaces;

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

    public IEnumerable<Edge> GetEdges()
    {
        return _edges.GetEdges();
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
}