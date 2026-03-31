using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a line or polyline relative to the local <see cref="Transform" />'s position.
/// </summary>
public sealed class LineRenderer : PositionalRenderer
{
    /// <summary>
    ///     Gets or sets the points defining the line or polyline relative to this renderer’s transform.
    /// </summary>
    public List<Vector> Points { get; set; } = [];

    /// <summary>
    ///     Gets or sets the color used to draw the lines.
    /// </summary>
    public Color Color { get; set; }

    private protected override void RenderAtPosition(PositionalRenderContext context)
    {
        for (int i = 1; i < Points.Count; i++)
        {
            DrawLine(Points[i - 1].RoundToInt(), Points[i].RoundToInt(), context);
        }
    }

    private void DrawLine(VectorInt start, VectorInt end, PositionalRenderContext context)
    {
        // Bresenham's line algorithm
        int x0 = start.X;
        int y0 = start.Y;
        int x1 = end.X;
        int y1 = end.Y;

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            context.Frame.Draw((x0, y0) + context.Offset.FloorToInt(), Color);

            if (x0 == x1 && y0 == y1)
            {
                break;
            }

            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
}