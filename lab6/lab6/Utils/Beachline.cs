using lab6.Interfaces;
using lab6.Models;

namespace lab6.Utils;

public class Beachline(IEventQueue eventQueue) : IBeachline
{
    private Arc Root { get; set; }

    public IEnumerable<Edge> GetAllEdges()
    {
        var edges = new HashSet<Edge>();

        TraverseArcs(Root, arc =>
        {
            if (arc.LeftEdge != null) edges.Add(arc.LeftEdge);
            if (arc.RightEdge != null) edges.Add(arc.RightEdge);
        });

        return edges;
    }

    public void FinishEdges(float bboxSize)
    {
        TraverseArcs(Root, arc =>
        {
            if (arc.LeftEdge?.End == null)
            {
                arc.LeftEdge.End = arc.LeftEdge.GetPointAt(bboxSize);
            }

            if (arc.RightEdge?.End == null)
            {
                arc.RightEdge.End = arc.RightEdge.GetPointAt(bboxSize);
            }
        });
    }

    private void TraverseArcs(Arc node, Action<Arc> action)
    {
        if (node == null) return;
        TraverseArcs(node.Left, action);
        action(node);
        TraverseArcs(node.Right, action);
    }


    public void Insert(Point site, float sweepLineY)
    {
        if (Root == null)
        {
            Root = new Arc(site);
            return;
        }

        var arcAbove = LocateArcAbove(site.X, sweepLineY);
        if (arcAbove == null)
            throw new Exception("Couldn't locate arc above site.");

        // Invalidate old circle event
        arcAbove.CircleEvent = null;

        // Create new arcs
        var arcLeft = new Arc(arcAbove.Site);
        var arcCenter = new Arc(site);
        var arcRight = new Arc(arcAbove.Site);

        // Link neighbors
        arcLeft.Prev = arcAbove.Prev;
        arcLeft.Next = arcCenter;
        arcCenter.Prev = arcLeft;
        arcCenter.Next = arcRight;
        arcRight.Prev = arcCenter;
        arcRight.Next = arcAbove.Next;

        if (arcLeft.Prev != null) arcLeft.Prev.Next = arcLeft;
        if (arcRight.Next != null) arcRight.Next.Prev = arcRight;

        // Replace arcAbove in tree
        ReplaceArcInTree(arcAbove, arcCenter);
        arcCenter.Left = arcLeft;
        arcCenter.Right = arcRight;
        arcLeft.Parent = arcCenter;
        arcRight.Parent = arcCenter;
        
        var left = site;                // The new site
        var right = arcAbove.Site;  

        // Compute perpendicular direction between sites
        var dx = right.X - left.X;
        var dy = right.Y - left.Y;
        var length = MathF.Sqrt(dx * dx + dy * dy);
        var direction = length == 0
            ? new Point(0, 0)
            : new Point(dy / length, -dx / length); // Perpendicular, unit length

        // Create and assign edges
        var edgeLeft = new Edge(site, direction, site, arcAbove.Site);
        var edgeRight = new Edge(site, direction, arcAbove.Site, site);

        arcCenter.LeftEdge = edgeLeft;
        arcCenter.RightEdge = edgeRight;
        arcLeft.RightEdge = edgeLeft;
        arcRight.LeftEdge = edgeRight;

        // Check for possible new circle events
        CheckCircleEvent(arcLeft, sweepLineY);
        CheckCircleEvent(arcRight, sweepLineY);
    }

    public void HandleCircleEvent(Event circleEvent, float sweepLineY)
    {
        var arc = circleEvent.DisappearingArc;
        if (arc == null || arc.Prev == null || arc.Next == null)
            return;

        // Create a vertex at the point where the arc disappears
        var vertex = circleEvent.Point;

        // Finalize the left and right edges of the disappearing arc
        arc.LeftEdge?.Let(e => e.End = vertex);
        arc.RightEdge?.Let(e => e.End = vertex);

        // Remove the arc from the beachline structure
        RemoveArc(arc);

        // Create a new edge between arc.Prev and arc.Next
        var a = arc.Prev.Site;
        var b = arc.Next.Site;
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;
        var direction = new Point(dy, -dx); // Perpendicular to line a→b

        var newEdge = new Edge(vertex, direction, a, b);
        arc.Prev.RightEdge = newEdge;
        arc.Next.LeftEdge = newEdge;

        // Invalidate circle events on neighbors
        arc.Prev.CircleEvent = null;
        arc.Next.CircleEvent = null;

        // Check for new circle events
        CheckCircleEvent(arc.Prev, sweepLineY);
        CheckCircleEvent(arc.Next, sweepLineY);
    }

    private void RemoveArc(Arc arc)
    {
        if (arc.Prev != null)
            arc.Prev.Next = arc.Next;
        if (arc.Next != null)
            arc.Next.Prev = arc.Prev;

        var parent = arc.Parent;
        var child = arc.Left ?? arc.Right;

        if (parent == null)
        {
            Root = child;
            if (child != null) child.Parent = null;
        }
        else
        {
            if (parent.Left == arc)
                parent.Left = child;
            else
                parent.Right = child;

            if (child != null)
                child.Parent = parent;
        }
    }


    private Arc LocateArcAbove(float x, float sweepLineY)
    {
        var node = Root;
        while (node.Left != null || node.Right != null)
        {
            float breakpointLeft = GetBreakpoint(node, true, sweepLineY);
            float breakpointRight = GetBreakpoint(node, false, sweepLineY);

            if (x < breakpointLeft)
                node = node.Left;
            else if (x > breakpointRight)
                node = node.Right;
            else
                break;
        }

        return node;
    }

    private float GetBreakpoint(Arc arc, bool isLeft, float directrix)
    {
        Point p = isLeft ? arc.Left?.Site ?? arc.Site : arc.Site;
        Point q = isLeft ? arc.Site : arc.Right?.Site ?? arc.Site;

        // Handle vertical case (equal y values)
        if (Math.Abs(p.Y - q.Y) < 1e-6)
            return (p.X + q.X) / 2;

        // Swap if necessary
        if (p.Y == directrix) return p.X;
        if (q.Y == directrix) return q.X;

        float z1 = 2 * (p.Y - directrix);
        float z2 = 2 * (q.Y - directrix);

        float a = 1 / z1 - 1 / z2;
        float b = -2 * (p.X / z1 - q.X / z2);
        float c = (p.X * p.X + p.Y * p.Y - directrix * directrix) / z1
                  - (q.X * q.X + q.Y * q.Y - directrix * directrix) / z2;

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0) return (p.X + q.X) / 2;

        float sqrtD = MathF.Sqrt(discriminant);
        float x1 = (-b + sqrtD) / (2 * a);
        float x2 = (-b - sqrtD) / (2 * a);

        return isLeft ? MathF.Min(x1, x2) : MathF.Max(x1, x2);
    }

    private void CheckCircleEvent(Arc arc, float sweepLineY)
    {
        if (arc.Prev == null || arc.Next == null)
            return;

        var a = arc.Prev.Site;
        var b = arc.Site;
        var c = arc.Next.Site;

        if (IsCounterClockwise(a, b, c) >= 0)
            return;

        var center = GetCircleCenter(a, b, c);
        if (center == null)
            return;

        float radius = Distance(center, b);
        float y = center.Y - radius;

        if (y >= sweepLineY)
            return;

        var circleEvent = new Event(EventType.Circle, center with { Y = y })
        {
            DisappearingArc = arc
        };

        arc.CircleEvent = circleEvent;
        eventQueue.Enqueue(circleEvent);
    }

    private static float IsCounterClockwise(Point a, Point b, Point c)
    {
        return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
    }

    private static float Distance(Point p1, Point p2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    private static Point? GetCircleCenter(Point a, Point b, Point c)
    {
        var A = b.X - a.X;
        var B = b.Y - a.Y;
        var C = c.X - a.X;
        var D = c.Y - a.Y;

        var E = A * (a.X + b.X) + B * (a.Y + b.Y);
        var F = C * (a.X + c.X) + D * (a.Y + c.Y);
        var G = 2 * (A * (c.Y - b.Y) - B * (c.X - b.X));

        if (Math.Abs(G) < 1e-10)
            return null;

        var cx = (D * E - B * F) / G;
        var cy = (A * F - C * E) / G;

        return new Point(cx, cy);
    }

    private void ReplaceArcInTree(Arc oldArc, Arc newArc)
    {
        if (oldArc.Parent == null)
        {
            Root = newArc;
        }
        else if (oldArc.Parent.Left == oldArc)
        {
            oldArc.Parent.Left = newArc;
        }
        else
        {
            oldArc.Parent.Right = newArc;
        }

        newArc.Parent = oldArc.Parent;
    }
}