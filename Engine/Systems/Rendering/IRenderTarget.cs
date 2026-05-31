using Termule.Engine.Types;

namespace Termule.Engine.Systems.Rendering;

/// <summary>
///     Denotes a type that can be rendered to by the <see cref="RenderSystem"/>.
/// </summary>
public interface IRenderTarget
{
    /// <summary>
    ///     Gets the position of the top-leftmost <see cref="Cell"/> of the
    ///     <see cref="IRenderTarget"/>.
    /// </summary>
    public VectorInt LowerBound { get; }

    /// <summary>
    ///     Gets the position of the bottom-rightmost <see cref="Cell"/> of the
    ///     <see cref="IRenderTarget"/>.
    /// </summary>
    public VectorInt UpperBound { get; }

    /// <summary>
    ///     Gets the size of this render target.
    /// </summary>
    public VectorInt Size => UpperBound - LowerBound;

    internal ref Cell GetCellRef(int x, int y);

    /// <summary>
    ///     Modifies a cell in this <see cref="IRenderTarget"/> .
    /// </summary>
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
    public void Draw(
        VectorInt pos,
        Color? color = null,
        char? glyph = null,
        Color? glyphColor = null,
        bool layerBoxDrawingChars = true)
    {
        if (
            pos.X < LowerBound.X
            || pos.X >= UpperBound.X
            || pos.Y < LowerBound.Y
            || pos.Y >= UpperBound.Y)
        {
            return;
        }

        ref Cell cell = ref GetCellRef(pos.X, pos.Y);

        if (color != null)
        {
            cell.Color = color.Value;
            cell.Glyph = '\0';
            cell.GlyphColor = default;
        }

        if (glyph != null)
        {
            if (layerBoxDrawingChars && glyphColor == cell.GlyphColor)
            {
                Connections connections = ConnectionsExtensions.FromGlyph(glyph.Value)
                    | ConnectionsExtensions.FromGlyph(cell.Glyph);

                glyph = connections.ToGlyph();
            }

            cell.Glyph = glyph.Value;
            cell.GlyphColor = default;
        }

        if (glyphColor != null)
        {
            cell.GlyphColor = glyphColor.Value;
        }
    }
}
