using lab6.BeachLine.Nodes;
using lab6.Graph;

namespace lab6.BeachLine;

public interface IBeachLine
{
    BeachNode? GetRoot();
    void SetRoot(BeachNode newRoot);
    InsertionResult InsertArc(Point point);
}