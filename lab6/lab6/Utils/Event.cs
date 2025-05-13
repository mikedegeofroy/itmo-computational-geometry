using lab6.Models;

namespace lab6.Utils;

public enum EventType
{
    Site,
    Circle
}

public class Event(EventType type, Point point) : IComparable<Event>
{
    public EventType Type { get; set; } = type;
    public Point Point { get; set; } = point; // For site or circle bottom
    public float Y => Point.Y;
    public Arc DisappearingArc { get; set; } // for circle events

    public int CompareTo(Event other)
    {
        // We want events with *higher Y* to come out first
        return other.Y.CompareTo(Y);
    }
}