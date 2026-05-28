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
    /// <param name="character">The character to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="characterColor">
    ///     The character color to set, or <c>null</c> to leave unchanged.
    /// </param>
    /// <param name="layerBoxDrawingChars">
    ///     Indicates that drawing Unicode box-drawing characters over existing box-drawing
    ///     characters of the same color should combine them.
    /// </param>
    public void Draw(
        VectorInt pos,
        Color? color = null,
        char? character = null,
        Color? characterColor = null,
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
            cell.Character = '\0';
            cell.CharColor = default;
        }

        if (character != null)
        {
            if (layerBoxDrawingChars && characterColor == cell.CharColor)
            {
                Connections connections = ConnectionsExtensions.FromChar(character.Value)
                    | ConnectionsExtensions.FromChar(cell.Character);

                character = connections.ToChar();
            }

            cell.Character = character.Value;
            cell.CharColor = default;
        }

        if (characterColor != null)
        {
            cell.CharColor = characterColor.Value;
        }
    }
}
