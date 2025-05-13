using lab6.BeachLine;
using lab6.Graph;
using lab6.Interfaces;

namespace lab6.Events;

public interface IEvent : IComparable<IEvent>
{
    Point Point { get; set; }
    void Handle(IEventQueue eventQueue, IBeachLine beachline, IGraph graph);
}