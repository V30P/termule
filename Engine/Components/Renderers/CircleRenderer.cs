using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a circle at the local <see cref="IPositionProvider" />'s position.
/// </summary>
public sealed class CircleRenderer : PositionalRenderer
{
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

    /// <summary>
    ///     Gets or sets the color to draw the circle in.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether circle cells should be duplicated horizontally.
    /// </summary>
    /// <remarks>
    ///     This is useful for making more round circles in the terminal.
    /// </remarks>
    public bool DoubleWide { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the interior cells of the
    ///     circle should be filled.
    /// </summary>
    public bool Filled { get; set; }

    private protected override void RenderPositionally(IRenderTarget target, Vector sub)
    {
        target.DrawCircle(
            (0, 0),
            Radius,
            Color,
            doubleWide: DoubleWide,
            biasRight: sub.X > 0.5f,
            filled: Filled
        );
    }
}
