using Termule.Engine.Components;
using Termule.Engine.Types;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Systems.Display;

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
    /// <param name="characterColor">The character color to set, or <c>null</c> to leave unchanged.</param>
    public void Draw(VectorInt pos, Color? color = null, char? character = null,
        Color? characterColor = null)
    {
        if (pos.X < 0 || pos.X >= Size.X || pos.Y < 0 || pos.Y >= Size.Y)
        {
            return;
        }

        ref Cell cell = ref Cells[pos.X, pos.Y];

        if (color is { } colorValue)
        {
            cell.Color = colorValue;
            cell.Char = '\0';
            cell.CharColor = default;
        }

        if (character is { } characterValue)
        {
            cell.Char = characterValue;
            cell.CharColor = default;
        }

        if (characterColor is { } characterColorValue)
        {
            cell.CharColor = characterColorValue;
        }
    }

    internal void Reset(Cell cell = default)
    {
        for (int x = 0; x < Size.X; x++)
        for (int y = 0; y < Size.Y; y++)
        {
            Cells[x, y] = cell;
        }
    }
}