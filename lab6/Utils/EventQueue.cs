using lab6.Events;
using lab6.Interfaces;

namespace lab6.Utils;

public class EventQueue : IEventQueue
{
    private readonly SortedSet<IEvent> _queue;

    public EventQueue()
    {
        _queue = new SortedSet<IEvent>(Comparer<IEvent>.Create((a, b) => a.CompareTo(b)));
    }

    public void Enqueue(IEvent e) => _queue.Add(e);

    public void Remove(IEvent e) => _queue.Remove(e);
    public IEvent? Peek() => _queue.FirstOrDefault();

    public IEvent Dequeue()
    {
        var e = _queue.First();
        _queue.Remove(e);
        return e;
    }

    public bool IsEmpty => _queue.Count == 0;
}