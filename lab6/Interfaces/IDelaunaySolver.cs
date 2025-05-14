using lab6.Graph;

namespace lab6.Interfaces;

public interface IDelaunaySolver
{
    IEnumerable<Edge> Solve();
}