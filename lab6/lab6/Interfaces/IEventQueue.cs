using lab6.Utils;

namespace lab6.Interfaces;

public interface IEventQueue
{
    void Enqueue(Event e);
    Event Dequeue();
    bool IsEmpty { get; }
}