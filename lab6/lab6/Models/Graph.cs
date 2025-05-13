using System.Collections.ObjectModel;

namespace lab6.Models;

public class Graph
{
    private readonly BisectorMap edges = new();
    private readonly HashSet<Point> sites = new();

    public void AddEdge(Edge e)
    {
        edges.Put(e.Start, e.End, e);
    }

    public Edge? GetEdgeBetweenSites(Point a, Point b)
    {
        return edges.Get(a, b);
    }

    public void AddSite(Point newSite)
    {
        sites.Add(newSite);
    }

    public IReadOnlyCollection<Point> GetSitePoints()
    {
        return new ReadOnlyCollection<Point>(sites.ToList());
    }

    public IEnumerable<Edge> EdgeStream()
    {
        return edges.Stream();
    }

    public override bool Equals(object? obj)
    {
        return obj is Graph g &&
               sites.SetEquals(g.sites) &&
               edges.Equals(g.edges);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(sites, edges);
    }
}