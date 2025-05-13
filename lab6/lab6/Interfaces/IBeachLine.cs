using lab6.Models;
using lab6.Utils;

namespace lab6.Interfaces;

public interface IBeachline
{
    void Insert(Point site, float sweepLineY);
    void HandleCircleEvent(Event circleEvent, float sweepLineY);
    void FinishEdges(float bboxSize);
    IEnumerable<Edge> GetAllEdges();
}