using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a line or polyline relative to the local <see cref="Transform" />'s position.
/// </summary>
public sealed class LineRenderer : PositionalRenderer
{
    /// <summary>
    ///     Gets or sets the points defining the line or polyline relative to this renderer’s
    ///     transform.
    /// </summary>
    public List<Vector> Points { get; set; } = [];

    /// <summary>
    ///     Gets or sets the color used to draw the lines.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether lines should be drawn with Unicode box-drawing characters.
    /// </summary>
    public bool UseBoxDrawingCharacters { get; set; }

    private protected override void RenderAtPosition(PositionalRenderContext context)
    {
        for (int i = 1; i < Points.Count; i++)
        {
            DrawLine(Points[i - 1].RoundToInt(), Points[i].RoundToInt(), context);
        }
    }

    private void DrawLine(VectorInt p1, VectorInt p2, PositionalRenderContext context)
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

                // When using box-drawing characters, steps must be orthogonal
                if (UseBoxDrawingCharacters)
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
            if (UseBoxDrawingCharacters)
            {
                Connections connections =
                    (prev != null ? GetConnection(prev.Value - curr.Value) : Connections.None) |
                    (next != null ? GetConnection(next.Value - curr.Value) : Connections.None);

                context.Frame.Draw(
                    context.Origin + curr.Value,
                    character: connections.ToChar(),
                    characterColor: Color
                );
            }
            else
            {
                context.Frame.Draw(context.Origin + curr.Value, Color);
            }
        }

        static Connections GetConnection(VectorInt displacement)
        {
            return displacement switch
            {
                { X: > 0 } => Connections.Right,
                { X: < 0 } => Connections.Left,
                { Y: > 0 } => Connections.Down,
                { Y: < 0 } => Connections.Up,
                _ => Connections.None
            };
        }
    }
}
