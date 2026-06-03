using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a line or polyline relative to the local <see cref="IPositionProvider" />'s
///     position.
/// </summary>
public sealed class LineRenderer : PositionalRenderer
{
    /// <summary>
    ///     Gets or sets the points defining the line or polyline relative to this renderer’s
    ///     <see cref="IPositionProvider" />.
    /// </summary>
    public List<Vector> Points { get; set; } = [];

    /// <summary>
    ///     Gets or sets the color used to draw the lines.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether lines should be drawn with Unicode box-drawing
    ///     glyphs.
    /// </summary>
    public bool UseBoxDrawingGlyphs { get; set; }

    private protected override void RenderPositionally(IRenderTarget target, Vector _)
    {
        for (int i = 1; i < Points.Count; i++)
        {
            DrawLine(Points[i - 1].RoundToInt(), Points[i].RoundToInt(), target);
        }
    }

    private void DrawLine(VectorInt p1, VectorInt p2, IRenderTarget target)
    {
        // Modified Bresenham's line algorithm
        int dx = Math.Abs(p2.X - p1.X);
        int dy = Math.Abs(p2.Y - p1.Y);
        int sx = p1.X < p2.X ? 1 : -1;
        int sy = p1.Y < p2.Y ? 1 : -1;
        int err = dx - dy;

        VectorInt? prev;
        VectorInt? curr = null;
        VectorInt? next = p1;
        while (next != null)
        {
            prev = curr;
            curr = next;
            next = null;

            int e2 = err * 2;
            if (curr != p2 && e2 > -dy)
            {
                err -= dy;
                next = new VectorInt(curr.Value.X + sx, curr.Value.Y);

                // When using box-drawing glyphs, steps must be orthogonal
                if (UseBoxDrawingGlyphs)
                {
                    DrawSegment();
                    continue;
                }
            }

            if (curr != p2 && e2 < dx)
            {
                err += dx;
                next = new VectorInt((next ?? curr).Value.X, (next ?? curr).Value.Y + sy);
            }

            DrawSegment();
        }

        void DrawSegment()
        {
            if (UseBoxDrawingGlyphs)
            {
                Connections connections =
                    (prev != null ? GetConnection(prev.Value - curr.Value) : Connections.None) |
                    (next != null ? GetConnection(next.Value - curr.Value) : Connections.None);

                target.Draw(
                    curr.Value,
                    glyph: connections.ToGlyph(),
                    glyphColor: Color
                );
            }
            else
            {
                target.Draw(curr.Value, Color);
            }
        }

        Connections GetConnection(VectorInt displacement)
        {
            return displacement switch
            {
                { X: > 0 } => Connections.Right,
                { X: < 0 } => Connections.Left,

                // World-space vertical connections must be flipped
                { Y: > 0 } => RenderInTargetSpace ? Connections.Down : Connections.Up,
                { Y: < 0 } => RenderInTargetSpace ? Connections.Up : Connections.Down,
                _ => Connections.None
            };
        }
    }
}
