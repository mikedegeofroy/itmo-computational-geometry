using lab6.Graph;

namespace lab6.BeachLine.Nodes;

public abstract class BeachNode
{
    private InnerBeachNode? _parent;

    public InnerBeachNode? Parent
    {
        get => _parent;
        set => _parent = value;
    }

    public abstract InsertionResult InsertArc(Point newSite);

    public abstract LeafBeachNode GetLeftmostLeaf();

    public abstract LeafBeachNode GetRightmostLeaf();

    public void ReplaceBy(BeachNode? node)
    {
        if (_parent == null) return;
        if (_parent.LeftChild == this)
        {
            _parent.LeftChild = node;
        }
        else
        {
            _parent.RightChild = node;
        }
    }
}