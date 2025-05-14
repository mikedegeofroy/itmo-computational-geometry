using lab6.Graph;

namespace lab6.Interfaces;

public interface ISvgRenderer
{
    void RenderToSvg(IEnumerable<Edge> edges, string path, double width = 400, double height = 400);
}