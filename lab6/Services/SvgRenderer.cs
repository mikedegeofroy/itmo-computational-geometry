using System.Globalization;
using System.Text;
using lab6.Graph;
using lab6.Interfaces;

namespace lab6.Services;

public class SvgRenderer : ISvgRenderer
{
    public void RenderToSvg(IEnumerable<Edge> edges, string path, double width = 400, double height = 400)
    {
        var sb = new StringBuilder();

        sb.AppendLine(
            $"<svg xmlns='http://www.w3.org/2000/svg' width='{width}' height='{height}' viewBox='0 0 {width} {height}'>");
        sb.AppendLine("<style>line { stroke: black; stroke-width: 1; }</style>");

        foreach (var edge in edges)
        {
            if (edge.A == null || edge.B == null)
                continue; // skip unfinished edges

            var x1 = edge.A.Location.X * 10;
            var y1 = height - edge.A.Location.Y * 10;
            var x2 = edge.B.Location.X * 10;
            var y2 = height - edge.B.Location.Y * 10;

            sb.AppendLine($"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' />");
        }

        sb.AppendLine("</svg>");
        File.WriteAllText(path, sb.ToString());
    }
}