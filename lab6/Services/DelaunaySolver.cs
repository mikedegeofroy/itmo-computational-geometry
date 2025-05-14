using lab6.Graph;
using lab6.Interfaces;

namespace lab6.Services;

public class DelaunaySolver(IGraph graph) : IDelaunaySolver
{
    public IEnumerable<Edge> Solve()
    {
        var seen = new HashSet<int>();

        foreach (var vorEdge in graph.GetEdges())
        {
            if (vorEdge.A == null || vorEdge.B == null)
                continue;

            var key = vorEdge.GetHashCode();

            if (!seen.Add(key))
                continue;

            yield return new Edge
            {
                A = new Vertex(vorEdge.Site1),
                B = new Vertex(vorEdge.Site2),
            };
        }
    }
}