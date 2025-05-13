using lab6.Utils;

namespace lab6.Models;

public class Arc
{
    public Point Site { get; set; }        // The focus of the parabola
    public Event CircleEvent { get; set; }                 // Potential circle event

    public Arc Parent { get; set; }                        // Parent in beachline tree
    public Arc Left { get; set; }                          // Left child
    public Arc Right { get; set; }                         // Right child

    public Arc Prev { get; set; }                          // Previous arc in linked list
    public Arc Next { get; set; }                          // Next arc in linked list

    public Edge LeftEdge { get; set; }                     // Left edge
    public Edge RightEdge { get; set; }                    // Right edge

    public Arc(Point site)
    {
        Site = site;
    }
}