using lab6.Models;

namespace lab6.Interfaces;

public interface ISvgRenderer
{
    void RenderToSvg(IEnumerable<Edge> edges, string path, float width = 800, float height = 800);
}