using lab6.Graph;

namespace lab6.Interfaces;

public interface IGraph
{
    IReadOnlyCollection<Point> GetSitePoints();
    Edge GetEdgeBetweenSites(Point lSite, Point cSite);
    IEnumerable<Edge> GetEdges();
    void AddEdge(Edge newEdge);
    void AddSite(Point site);
}