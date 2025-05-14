using lab6.BeachLine;
using lab6.Events;
using lab6.Graph;
using lab6.Interfaces;
using lab6.Utils;

namespace lab6.Services;

public class VoronoiSolver(IEventQueue eventQueue, IBeachLine beachline, IGraph graph) : IVoronoiSolver
{
    public void Run(IEnumerable<Point> sites)
    {
        foreach (var site in sites)
        {
            eventQueue.Enqueue(new SiteEvent(site));
            graph.AddSite(site);
        }

        var sweep = double.PositiveInfinity;
        while (!eventQueue.IsEmpty)
        {
            var current = eventQueue.Dequeue();
            Console.WriteLine(sweep);
            current.Handle(eventQueue, beachline, graph);
            sweep = current.Point.Y;
        }
    }

    public IEnumerable<Edge> GetEdges()
    {
        return graph.GetEdges();
    }
}