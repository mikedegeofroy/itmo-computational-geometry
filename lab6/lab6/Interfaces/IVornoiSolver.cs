using lab6.Models;

namespace lab6.Interfaces;

public interface IVoronoiSolver
{
    void Run(IEnumerable<Point> sites);
    IEnumerable<Edge> GetEdges();
}