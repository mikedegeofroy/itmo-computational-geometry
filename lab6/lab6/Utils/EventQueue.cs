using lab6.Interfaces;

namespace lab6.Utils;

public class EventQueue : IEventQueue
{
    private readonly SortedSet<Event> _queue;

    public EventQueue()
    {
        _queue = new SortedSet<Event>(Comparer<Event>.Create((a, b) =>
        {
            var cmp = b.Y.CompareTo(a.Y); // descending by Y
            return cmp == 0 ? a.Point.X.CompareTo(b.Point.X) : // tie-breaker
                cmp;
        }));
    }

    public void Enqueue(Event e) => _queue.Add(e);

    public Event Dequeue()
    {
        var e = _queue.First();
        _queue.Remove(e);
        return e;
    }

    public bool IsEmpty => _queue.Count == 0;
}