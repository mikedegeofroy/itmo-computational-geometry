using lab6.Graph;

namespace lab6.BeachLine.Nodes;

public class InnerBeachNode : BeachNode
{
    public BeachNode? LeftChild { get; set; }
    public BeachNode? RightChild { get; set; }

    public InnerBeachNode()
    {
    }

    public InnerBeachNode(BeachNode? leftChild, BeachNode? rightChild)
    {
        LeftChild = leftChild;
        leftChild.Parent = this;
        RightChild = rightChild;
        rightChild.Parent = this;
    }

    public override InsertionResult InsertArc(Point newSite)
    {
        var l = LeftChild.GetRightmostLeaf().Site;
        var r = RightChild.GetLeftmostLeaf().Site;

        var lxOld = l.X;
        r = new Point(r.X - l.X, r.Y - newSite.Y);
        l = new Point(0, l.Y - newSite.Y);

        double x;

        if (Math.Abs(l.Y - r.Y) < 0.01)
        {
            x = r.X / 2.0;
        }
        else if (l.Y == 0.0)
        {
            x = l.X;
        }
        else if (r.Y == 0.0)
        {
            x = r.X;
        }
        else
        {
            x = (l.Y * r.X - Math.Sqrt(l.Y * r.Y * (Math.Pow(l.Y - r.Y, 2) + Math.Pow(r.X, 2)))) / (l.Y - r.Y);
        }

        x += lxOld;

        return newSite.X < x ? LeftChild.InsertArc(newSite) : RightChild.InsertArc(newSite);
    }

    public override LeafBeachNode GetLeftmostLeaf() => LeftChild.GetLeftmostLeaf();

    public override LeafBeachNode GetRightmostLeaf() => RightChild.GetRightmostLeaf();

    public override bool Equals(object? obj)
    {
        return obj is InnerBeachNode other &&
               Equals(LeftChild, other.LeftChild) &&
               Equals(RightChild, other.RightChild);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LeftChild, RightChild);
    }
}