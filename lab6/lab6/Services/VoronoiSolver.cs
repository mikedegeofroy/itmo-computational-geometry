using lab6.Interfaces;
using lab6.Models;
using lab6.Utils;

namespace lab6.Services;

public class VoronoiSolver(IEventQueue eventQueue, IBeachline beachline) : IVoronoiSolver
{
    public void Run(IEnumerable<Point> sites)
    {
        foreach (var site in sites)
        {
            eventQueue.Enqueue(new Event(EventType.Site, site));
        }

        while (!eventQueue.IsEmpty)
        {
            var current = eventQueue.Dequeue();

            if (current.Type == EventType.Site)
            {
                beachline.Insert(current.Point, current.Point.Y);
            }
            else if (current.Type == EventType.Circle)
            {
                beachline.HandleCircleEvent(current, current.Point.Y);
            }
        }

        beachline.FinishEdges(10000);
    }

    public IEnumerable<Edge> GetEdges()
    {
        return beachline.GetAllEdges();
    }
}