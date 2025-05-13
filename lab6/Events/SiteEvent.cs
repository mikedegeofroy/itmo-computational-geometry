using lab6.BeachLine;
using lab6.Graph;
using lab6.Interfaces;

namespace lab6.Events;

public class SiteEvent(Point point) : IEvent
{
    public int CompareTo(IEvent other)
    {
        return -Point.Y.CompareTo(other.Point.Y);
    }

    public Point Point { get; set; } = point;

    public void Handle(IEventQueue eventQueue, IBeachLine beachline, IGraph graph)
    {
        var result = beachline.InsertArc(Point);

        switch (result)
        {
            case InsertionResult.SplitOccurred split:
            {
                graph.AddEdge(new Edge(split.SplitLeaf.Site, Point));
                foreach (var subscriber in split.SplitLeaf.Subscribers)
                {
                    eventQueue.Remove(subscriber);
                }

                split.NewLeaf?.AddCircleEvents(eventQueue.Enqueue, Point.Y);
                break;
            }
            case InsertionResult.NewLeafOnly only:
                only.NewLeaf?.AddCircleEvents(eventQueue.Enqueue, Point.Y);
                break;
        }
    }
}