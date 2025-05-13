using lab6.Graph;

namespace lab6.Interfaces;

public interface ISvgRenderer
{
    void RenderToSvg(IEnumerable<Edge> edges, string path, float width = 400, float height = 400);
}