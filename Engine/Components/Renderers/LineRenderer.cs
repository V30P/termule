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
            target.DrawLine(
                Points[i - 1].RoundToInt(),
                Points[i].RoundToInt(),
                Color,
                useBoxDrawingGlyphs: UseBoxDrawingGlyphs,
                flipVerticalGlyphConnections: !RenderInTargetSpace
            );
        }
    }
}
