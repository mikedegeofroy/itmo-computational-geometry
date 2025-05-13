using lab6.Events;
using lab6.Utils;

namespace lab6.Interfaces;

public interface IEventQueue
{
    void Enqueue(IEvent e);
    void Remove(IEvent e);
    IEvent? Peek();
    IEvent Dequeue();
    bool IsEmpty { get; }
}