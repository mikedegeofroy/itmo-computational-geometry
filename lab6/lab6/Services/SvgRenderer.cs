using System.Globalization;
using System.Text;
using lab6.Models;

namespace lab6.Services;

public static class SvgRenderer
{
    public static void RenderToSvg(IEnumerable<Edge> edges, string path, float width = 800, float height = 800)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<svg xmlns='http://www.w3.org/2000/svg' width='{width}' height='{height}' viewBox='0 0 {width} {height}'>");
        sb.AppendLine("<style>line { stroke: black; stroke-width: 1; }</style>");

        foreach (var edge in edges)
        {
            if (edge.Start is null || edge.End is null) continue;

            var x1 = edge.Start.X.ToString(CultureInfo.InvariantCulture);
            var y1 = (height - edge.Start.Y).ToString(CultureInfo.InvariantCulture);
            var x2 = edge.End.X.ToString(CultureInfo.InvariantCulture);
            var y2 = (height - edge.End.Y).ToString(CultureInfo.InvariantCulture);

            sb.AppendLine($"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' />");
        }

        sb.AppendLine("</svg>");
        File.WriteAllText(path, sb.ToString());
    }
}
