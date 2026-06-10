using Termule.Engine.Types;

namespace Termule.Engine.Systems.Rendering;

/// <summary>
///     A collection of extension methods for drawing to <see cref="IRenderTarget"/>s.
/// </summary>
public static class Drawing
{
    /// <summary>
    ///     Draws to a cell on this <see cref="IRenderTarget"/>.
    /// </summary>
    /// <param name="target">The target to draw to.</param>
    /// <param name="pos">The position of the cell.</param>
    /// <param name="color">The color to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="glyph">The glyph to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="glyphColor">
    ///     The glyph color to set, or <c>null</c> to leave unchanged.
    /// </param>
    /// <param name="layerBoxDrawingChars">
    ///     Indicates that drawing Unicode box-drawing glyphs over existing box-drawing
    ///     glyphs of the same color should combine them.
    /// </param>
    public static void Draw(
        this IRenderTarget target,
        VectorInt pos,
        Color? color = null,
        char? glyph = null,
        Color? glyphColor = null,
        bool layerBoxDrawingChars = true)
    {
        if (
            pos.X < target.LowerBound.X
            || pos.X >= target.UpperBound.X
            || pos.Y < target.LowerBound.Y
            || pos.Y >= target.UpperBound.Y)
        {
            return;
        }

        ref Cell cell = ref target[pos.X, pos.Y];

        if (color.HasValue)
        {
            cell.Color = color.Value;
            cell.Glyph = '\0';
            cell.GlyphColor = default;
        }

        if (glyph.HasValue)
        {
            if (layerBoxDrawingChars && glyphColor == cell.GlyphColor)
            {
                Connections connections = ConnectionsConversions.FromGlyph(glyph.Value)
                    | ConnectionsConversions.FromGlyph(cell.Glyph);

                glyph = connections.ToGlyph();
            }

            cell.Glyph = glyph.Value;
            cell.GlyphColor = default;
        }

        if (glyphColor.HasValue)
        {
            cell.GlyphColor = glyphColor.Value;
        }
    }

    /// <summary>
    ///     Draws a piece of <see cref="IContent"/> to this <see cref="IRenderTarget"/>.
    /// </summary>
    /// <param name="target">The target to draw to.</param>
    /// <param name="pos">The position to start drawing at.</param>
    /// <param name="content">The <see cref="IContent"/> to draw.</param>
    /// <param name="flipX">
    ///     Indicates that the content should be drawn flipped on the x-axis.
    /// </param>
    /// <param name="flipY">
    ///     Indicates that the content should be drawn flipped on the y-axis.
    /// </param>
    public static void DrawContent(
        this IRenderTarget target,
        VectorInt pos,
        IContent content,
        bool flipX = false,
        bool flipY = false)
    {
        for (int x = 0; x < content.Size.X; x++)
        {
            for (int y = 0; y < content.Size.Y; y++)
            {
                Cell cell = content[x, y];
                target.Draw(
                    pos + (
                        flipX ? (content.Size.X - x - 1) : x,
                        flipY ? (content.Size.Y - y - 1) : y
                    ),
                    cell.Color != BasicColor.Default ? cell.Color : null,
                    cell.Glyph != '\0' ? cell.Glyph : null,
                    cell.GlyphColor != BasicColor.Default ? cell.GlyphColor : null
                );
            }
        }
    }

    /// <summary>
    ///     Draws a line to this <see cref="IRenderTarget"/>.
    /// </summary>
    /// <param name="target">The target to draw to.</param>
    /// <param name="start">The beginning of the line.</param>
    /// <param name="finish">The end of the line.</param>
    /// <param name="color">The color to draw the line in.</param>
    /// <param name="useBoxDrawingGlyphs">
    ///     Indicates that lines should be drawn with Unicode box-drawing glyphs.
    /// </param>
    /// <param name="flipVerticalGlyphConnections">
    ///     Indicates that the vertical connections of box-drawing glyphs should be flipped.
    ///     This is useful when working in spaces with inverted y mapping.
    /// </param>
    public static void DrawLine(
        this IRenderTarget target,
        VectorInt start,
        VectorInt finish,
        Color color,
        bool useBoxDrawingGlyphs = false,
        bool flipVerticalGlyphConnections = false)
    {
        // Modified Bresenham's line algorithm
        int dx = Math.Abs(finish.X - start.X);
        int dy = Math.Abs(finish.Y - start.Y);
        int sx = start.X < finish.X ? 1 : -1;
        int sy = start.Y < finish.Y ? 1 : -1;
        int err = dx - dy;

        VectorInt? prev;
        VectorInt? curr = null;
        VectorInt? next = start;
        while (next != null)
        {
            prev = curr;
            curr = next;
            next = null;

            int e2 = err * 2;
            if (curr != finish && e2 > -dy)
            {
                err -= dy;
                next = new VectorInt(curr.Value.X + sx, curr.Value.Y);

                // When using box-drawing glyphs, steps must be orthogonal
                if (useBoxDrawingGlyphs)
                {
                    DrawSegment();
                    continue;
                }
            }

            if (curr != finish && e2 < dx)
            {
                err += dx;
                next = new VectorInt((next ?? curr).Value.X, (next ?? curr).Value.Y + sy);
            }

            DrawSegment();
        }

        void DrawSegment()
        {
            if (useBoxDrawingGlyphs)
            {
                Connections connections =
                    (prev != null ? GetConnection(prev.Value - curr.Value) : Connections.None) |
                    (next != null ? GetConnection(next.Value - curr.Value) : Connections.None);

                target.Draw(
                    curr.Value,
                    glyph: connections.ToGlyph(),
                    glyphColor: color
                );
            }
            else
            {
                target.Draw(curr.Value, color);
            }

            Connections GetConnection(VectorInt displacement)
            {
                return displacement switch
                {
                    { X: > 0 } => Connections.Right,
                    { X: < 0 } => Connections.Left,

                    // World-space vertical connections must be flipped
                    { Y: > 0 } => flipVerticalGlyphConnections ? Connections.Up : Connections.Down,
                    { Y: < 0 } => flipVerticalGlyphConnections ? Connections.Down : Connections.Up,
                    _ => Connections.None
                };
            }
        }
    }

    /// <summary>
    ///     Draws a circle to this <see cref="IRenderTarget"/>.
    /// </summary>
    /// <param name="target">The target to draw to.</param>
    /// <param name="pos">The position to center the circle on.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="color">The color to draw the circle in.</param>
    /// <param name="doubleWide">
    ///     Indicates that circle cells should be duplicated horizontally.
    ///     This is useful for creating rounder circles in the terminal.
    /// </param>
    /// <param name="biasRight">
    ///     Indicates that the circle should be biased to the right if it is not centered on a
    ///     a single cell.
    /// </param>
    /// <param name="filled">
    ///     Indicates that the interior cells of the circle should be filled.
    /// </param>
    public static void DrawCircle(
        this IRenderTarget target,
        VectorInt pos,
        float radius,
        Color color,
        bool doubleWide = false,
        bool biasRight = true,
        bool filled = false)
    {
        if (radius < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(radius),
                radius,
                "Radius cannot be negative"
            );
        }

        // Modified midpoint circle algorithm
        int y = (int) radius;
        int p = (int) (1 - radius);
        for (int x = 0; x <= y; x++)
        {
            DrawMidpointTransformations();
            if (filled)
            {
                FillHorizontals();
            }

            if (p < 0)
            {
                p += (2 * x) + 1;
            }
            else
            {
                y--;
                p = p + (2 * (x - y)) + 1;
            }

            void DrawMidpointTransformations()
            {
                DrawPoint(x, y);
                DrawPoint(x, -y);
                DrawPoint(y, -x);
                DrawPoint(-y, -x);
                DrawPoint(-x, -y);
                DrawPoint(-x, y);
                DrawPoint(-y, x);
                DrawPoint(y, x);
            }

            void FillHorizontals()
            {
                for (int ix = -x + 1; ix < x; ix++)
                {
                    DrawPoint(ix, y);
                    DrawPoint(ix, -y);
                }

                for (int ix = -y + 1; ix < y; ix++)
                {
                    DrawPoint(ix, x);
                    DrawPoint(ix, -x);
                }
            }
        }

        void DrawPoint(int x, int y)
        {
            if (!doubleWide)
            {
                target.Draw(pos + (x, y), color);
            }
            else
            {
                VectorInt widenedPos = pos + (x * 2, y);
                target.Draw(widenedPos, color);
                target.Draw(widenedPos + (biasRight ? (1, 0) : (-1, 0)), color);
            }
        }
    }
}
