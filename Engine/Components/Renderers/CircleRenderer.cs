using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a circle at the local <see cref="Transform" />'s position.
/// </summary>
public sealed class CircleRenderer : PositionalRenderer
{
    /// <summary>
    ///     Gets or sets the color to draw the circle in.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the interior cells of the
    ///     circle should be filled.
    /// </summary>
    public bool Filled { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether circle cells should be duplicated horizontally.
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
                throw new ArgumentOutOfRangeException(
                    nameof(Radius),
                    value,
                    "Radius cannot be negative"
                );
            }

            field = value;
        }
    }

    private protected override void RenderPositionally(IRenderTarget target, Vector sub)
    {
        // Midpoint circle algorithm
        int y = (int) Radius;
        int p = (int) (1 - Radius);
        for (int x = 0; x <= y; x++)
        {
            DrawMidpointTransformations((x, y), target, sub);
            if (Filled)
            {
                FillHorizontals((x, y), target, sub);
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
        }
    }

    private void DrawMidpointTransformations(VectorInt pos, IRenderTarget target, Vector sub)
    {
        DrawPoint((pos.X, pos.Y), target, sub);
        DrawPoint((pos.X, -pos.Y), target, sub);
        DrawPoint((pos.Y, -pos.X), target, sub);
        DrawPoint((-pos.Y, -pos.X), target, sub);
        DrawPoint((-pos.X, -pos.Y), target, sub);
        DrawPoint((-pos.X, pos.Y), target, sub);
        DrawPoint((-pos.Y, pos.X), target, sub);
        DrawPoint((pos.Y, pos.X), target, sub);
    }

    private void FillHorizontals(VectorInt pos, IRenderTarget target, Vector sub)
    {
        for (int x = -pos.X + 1; x < pos.X; x++)
        {
            DrawPoint((x, pos.Y), target, sub);
            DrawPoint((x, -pos.Y), target, sub);
        }

        for (int x = -pos.Y + 1; x < pos.Y; x++)
        {
            DrawPoint((x, pos.X), target, sub);
            DrawPoint((x, -pos.X), target, sub);
        }
    }

    private void DrawPoint(VectorInt pos, IRenderTarget target, Vector sub)
    {
        if (!DoubleWide)
        {
            target.Draw(pos, Color);
        }
        else
        {
            VectorInt widenedPos = (pos.X * 2, pos.Y);
            target.Draw(widenedPos, Color);

            target.Draw(widenedPos + (sub.X > 0.5f ? (1, 0) : (-1, 0)), Color);
        }
    }
}
