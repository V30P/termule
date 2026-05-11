using Termule.Engine.Components;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Rendering;

/// <summary>
///     Content implementation that <see cref="Renderer" />s draw to during the render process.
/// </summary>
public sealed class FrameBuffer : Image
{
    internal FrameBuffer(int width, int height) : base(width, height)
    {
    }

    /// <summary>
    ///     Modifies a cell in this frame buffer.
    /// </summary>
    /// <param name="pos">The position of the cell.</param>
    /// <param name="color">The color to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="character">The character to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="characterColor">
    ///     The character color to set, or <c>null</c> to leave unchanged.
    /// </param>
    /// <param name="layerBoxDrawingChars">
    ///     Indicates that drawing unicode box-drawing characters over existing box-drawing
    ///     characters of the same color should combine them.
    /// </param>
    public void Draw(
        VectorInt pos,
        Color? color = null,
        char? character = null,
        Color? characterColor = null,
        bool layerBoxDrawingChars = true)
    {
        if (pos.X < 0 || pos.X >= Size.X || pos.Y < 0 || pos.Y >= Size.Y)
        {
            return;
        }

        ref Cell cell = ref Cells[pos.X, pos.Y];

        if (color != null)
        {
            cell.Color = color.Value;
            cell.Char = '\0';
            cell.CharColor = default;
        }

        if (character != null)
        {
            if (layerBoxDrawingChars && characterColor == this[pos.X, pos.Y].CharColor)
            {
                Connections connections = Connections.FromChar(character.Value)
                                          | Connections.FromChar(this[pos.X, pos.Y].Char);

                character = connections.ToChar();
            }

            cell.Char = character.Value;
            cell.CharColor = default;
        }

        if (characterColor != null)
        {
            cell.CharColor = characterColor.Value;
        }
    }

    internal void Reset(Cell cell = default)
    {
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                Cells[x, y] = cell;
            }
        }
    }
}