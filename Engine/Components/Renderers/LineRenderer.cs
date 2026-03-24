using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a line or polyline relative to the local <see cref="Transform" />'s position.
/// </summary>
public sealed class LineRenderer : TransformRenderer
{
    /// <summary>
    ///     Gets or sets the points defining the line or polyline relative to this renderer’s transform.
    /// </summary>
    public List<Vector> Points { get; set; } = [];

    /// <summary>
    ///     Gets or sets the color used to draw the lines.
    /// </summary>
    public Color Color { get; set; }

    private protected override void RenderAtPosition(FrameBuffer frame, Vector frameSpacePos)
    {
        for (var i = 1; i < Points.Count; i++)
        {
            DrawLine(Points[i - 1].RoundToInt(), Points[i].RoundToInt(), frame, frameSpacePos);
        }
    }

    private void DrawLine(VectorInt start, VectorInt end, FrameBuffer frame, Vector offset)
    {
        // Bresenham's line algorithm
        var x0 = start.X;
        var y0 = start.Y;
        var x1 = end.X;
        var y1 = end.Y;

        var dx = Math.Abs(x1 - x0);
        var dy = Math.Abs(y1 - y0);
        var sx = x0 < x1 ? 1 : -1;
        var sy = y0 < y1 ? 1 : -1;
        var err = dx - dy;

        while (true)
        {
            frame.Draw((x0, y0) + offset.FloorToInt(), Color);

            if (x0 == x1 && y0 == y1)
            {
                break;
            }

            var e2 = err * 2;
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