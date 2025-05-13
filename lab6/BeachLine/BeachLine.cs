using lab6.BeachLine.Nodes;
using lab6.Graph;

namespace lab6.BeachLine;

public class BeachLine : IBeachLine
{
    private InnerBeachNode root { get; set; } = new();

    public BeachNode? GetRoot()
    {
        return root.LeftChild;
    }

    public void SetRoot(BeachNode newRoot)
    {
        root.LeftChild = newRoot;
    }

    public InsertionResult InsertArc(Point point)
    {
        var rootNode = GetRoot();
        if (rootNode != null) return rootNode.InsertArc(point);

        LeafBeachNode leafNode = new(point);
        SetRoot(leafNode);
        return new InsertionResult.NewLeafOnly(leafNode);
    }
}