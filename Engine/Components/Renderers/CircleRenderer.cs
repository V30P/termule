using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a circle at the local <see cref="Transform" />'s position.
/// </summary>
public sealed class CircleRenderer : TransformRenderer
{
    /// <summary>
    ///     Gets or sets the color to render the circle with.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether all cells inside the circle should be filled.
    /// </summary>
    public bool Filled { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether all circle cells should be duplicated horizontally.
    /// </summary>
    /// <remarks>
    ///     This is useful for making more round circles in the terminal.
    /// </remarks>
    public bool DoubleWide { get; set; }

    /// <summary>
    ///     Gets or sets the radius of the circle to render.
    /// </summary>
    public float Radius
    {
        get;

        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Radius), value, "Radius cannot be negative");
            }

            field = value;
        }
    }

    private protected override void RenderAtPosition(FrameBuffer frame, Vector frameSpacePos)
    {
        // Midpoint circle algorithm
        var y = (int) Radius;
        var p = (int) (1 - Radius);
        for (var x = 0; x <= y; x++)
        {
            DrawMidpointTransformations((x, y), frame, frameSpacePos);
            if (Filled)
            {
                FillHorizontals((x, y), frame, frameSpacePos);
            }
            
            p += 2 * (p < 0 ? 2 * x : x - --y) + 1;
        }
    }

    private void DrawMidpointTransformations(VectorInt pos, FrameBuffer frame, Vector offset)
    {
        DrawPoint((pos.X, pos.Y), frame, offset);
        DrawPoint((pos.X, -pos.Y), frame, offset);
        DrawPoint((pos.Y, -pos.X), frame, offset);
        DrawPoint((-pos.Y, -pos.X), frame, offset);
        DrawPoint((-pos.X, -pos.Y), frame, offset);
        DrawPoint((-pos.X, pos.Y), frame, offset);
        DrawPoint((-pos.Y, pos.X), frame, offset);
        DrawPoint((pos.Y, pos.X), frame, offset);
    }

    private void FillHorizontals(VectorInt pos, FrameBuffer frame, Vector offset)
    {
        for (var x = -pos.X + 1; x < pos.X; x++)
        {
            DrawPoint((x, pos.Y), frame, offset);
            DrawPoint((x, -pos.Y), frame, offset);
        }

        for (var x = -pos.Y + 1; x < pos.Y; x++)
        {
            DrawPoint((x, pos.X), frame, offset);
            DrawPoint((x, -pos.X), frame, offset);
        }
    }

    private void DrawPoint(VectorInt pos, FrameBuffer frame, Vector offset)
    {
        if (!DoubleWide)
        {
            var frameSpacePos = (pos + offset).FloorToInt();
            frame.Draw(frameSpacePos, Color);
        }
        else
        {
            var widenedPos = ((pos.X * 2, pos.Y) + offset).FloorToInt();
            frame.Draw(widenedPos, Color);
            
            var fraction = offset.X - (float)Math.Truncate(offset.X);
            frame.Draw(widenedPos + (fraction > 0.5f ? (1, 0) : (-1, 0)), Color);
        }
    }
}