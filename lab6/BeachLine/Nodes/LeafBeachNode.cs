using System.Collections.ObjectModel;
using lab6.Events;
using lab6.Graph;

namespace lab6.BeachLine.Nodes;

public class LeafBeachNode(Point site) : BeachNode
{
    private readonly Point _site = site ?? throw new ArgumentNullException(nameof(site));
    private readonly List<VertexEvent> _subscribedEvents = new();

    public Point Site => _site;

    private LeafBeachNode Copy() => new(_site);

    public override InsertionResult InsertArc(Point newSite)
    {
        var newLeaf = new LeafBeachNode(newSite);

        BeachNode? replacement;

        if (newSite.Y == _site.Y)
        {
            replacement = newSite.X < _site.X
                ? new InnerBeachNode(newLeaf, Copy())
                : new InnerBeachNode(Copy(), newLeaf);
        }
        else
        {
            replacement = newSite.X < _site.X
                ? new InnerBeachNode(new InnerBeachNode(Copy(), newLeaf), Copy())
                : new InnerBeachNode(Copy(), new InnerBeachNode(newLeaf, Copy()));
        }

        ReplaceBy(replacement);
        Parent = null; // Disconnect this node from the tree

        return new InsertionResult.SplitOccurred(this, newLeaf);
    }

    public void Remove()
    {
        var parent = Parent!;
        var sibling = ReferenceEquals(parent.LeftChild, this)
            ? parent.RightChild
            : parent.LeftChild;

        parent.ReplaceBy(sibling);
        Parent = null;
    }

    public override LeafBeachNode GetLeftmostLeaf() => this;

    public override LeafBeachNode GetRightmostLeaf() => this;

    public LeafBeachNode? GetLeftNeighbor()
    {
        var node = this as BeachNode;
        var parent = node.Parent;

        // Go up until we are a right child
        while (parent != null && ReferenceEquals(parent.LeftChild, node))
        {
            node = parent;
            parent = parent.Parent;
        }

        if (parent == null)
            return null; // No neighbor on the left

        // Go to the sibling and then as far right as possible
        var neighbor = parent.LeftChild;
        while (neighbor is InnerBeachNode inner)
            neighbor = inner.RightChild;

        return neighbor as LeafBeachNode;
    }

    public LeafBeachNode? GetRightNeighbor()
    {
        var node = this as BeachNode;
        var parent = node.Parent;

        // Go up until we are a left child
        while (parent != null && ReferenceEquals(parent.RightChild, node))
        {
            node = parent;
            parent = parent.Parent;
        }

        if (parent == null)
            return null; // No neighbor on the right

        // Go to the sibling and then as far left as possible
        var neighbor = parent.RightChild;
        while (neighbor is InnerBeachNode inner)
            neighbor = inner.LeftChild;

        return neighbor as LeafBeachNode;
    }

    private static VertexEvent? BuildEvent(LeafBeachNode center)
    {
        var left = center.GetLeftNeighbor();
        var right = center.GetRightNeighbor();

        if (left == null || right == null)
        {
            return null;
        }

        var vertexEvent = VertexEvent.Build(left, center, right);

        return vertexEvent;
    }


    public void AddCircleEvents(Action<IEvent> queue, double sweepY)
    {
        TryBuildAndEnqueue(GetLeftNeighbor()?.GetLeftNeighbor(), queue);
        TryBuildAndEnqueue(GetLeftNeighbor(), queue);
        TryBuildAndEnqueue(this, queue);
        TryBuildAndEnqueue(GetRightNeighbor(), queue);
        TryBuildAndEnqueue(GetRightNeighbor()?.GetRightNeighbor(), queue);
    }

    private void TryBuildAndEnqueue(LeafBeachNode? center, Action<IEvent> queue)
    {
        var evt = center != null ? BuildEvent(center) : null;
        if (evt != null)
            queue(evt);
    }

    public void Subscribe(VertexEvent e)
    {
        _subscribedEvents.Add(e);
    }

    public IReadOnlyList<VertexEvent> Subscribers => new ReadOnlyCollection<VertexEvent>(_subscribedEvents);
}