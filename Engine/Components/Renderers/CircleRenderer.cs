using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a circle at the local <see cref="Transform" />'s position.
/// </summary>
public sealed class CircleRenderer : PositionalRenderer
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

    private protected override void RenderAtPosition(PositionalRenderContext context)
    {
        // Midpoint circle algorithm
        int y = (int)Radius;
        int p = (int)(1 - Radius);
        for (int x = 0; x <= y; x++)
        {
            DrawMidpointTransformations((x, y), context);
            if (Filled)
            {
                FillHorizontals((x, y), context);
            }

            p += (2 * (p < 0 ? 2 * x : x - --y)) + 1;
        }
    }

    private void DrawMidpointTransformations(VectorInt pos, PositionalRenderContext context)
    {
        DrawPoint((pos.X, pos.Y), context);
        DrawPoint((pos.X, -pos.Y), context);
        DrawPoint((pos.Y, -pos.X), context);
        DrawPoint((-pos.Y, -pos.X), context);
        DrawPoint((-pos.X, -pos.Y), context);
        DrawPoint((-pos.X, pos.Y), context);
        DrawPoint((-pos.Y, pos.X), context);
        DrawPoint((pos.Y, pos.X), context);
    }

    private void FillHorizontals(VectorInt pos, PositionalRenderContext context)
    {
        for (int x = -pos.X + 1; x < pos.X; x++)
        {
            DrawPoint((x, pos.Y), context);
            DrawPoint((x, -pos.Y), context);
        }

        for (int x = -pos.Y + 1; x < pos.Y; x++)
        {
            DrawPoint((x, pos.X), context);
            DrawPoint((x, -pos.X), context);
        }
    }

    private void DrawPoint(VectorInt pos, PositionalRenderContext context)
    {
        if (!DoubleWide)
        {
            VectorInt frameSpacePos = pos + context.Origin;
            context.Frame.Draw(frameSpacePos, Color);
        }
        else
        {
            VectorInt widenedPos = (pos.X * 2, pos.Y) + context.Origin;
            context.Frame.Draw(widenedPos, Color);

            context.Frame.Draw(widenedPos + (context.Offset.X > 0 ? (1, 0) : (-1, 0)), Color);
        }
    }
}