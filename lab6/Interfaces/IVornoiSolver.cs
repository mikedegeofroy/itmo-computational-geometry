using lab6.Graph;

namespace lab6.Interfaces;

public interface IVoronoiSolver
{
    void Run(IEnumerable<Point> sites);
    IEnumerable<Edge> GetEdges();
}