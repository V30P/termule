using Termule.Engine.Components;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Content that <see cref="Renderer" />s contribute to during the render process.
/// </summary>
public sealed class FrameBuffer : Content
{
    /// <summary>
    ///     Gets the <see cref="Cell" /> at the given position.
    /// </summary>
    /// <param name="x">The X position of the cell to get.</param>
    /// <param name="y">The Y position of the cell to get.</param>
    /// <returns>The <see cref="Cell" /> at the specified position.</returns>
    public Cell this[int x, int y] => Cells[x, y];

    internal FrameBuffer(int width, int height)
        : base(width, height)
    {
    }

    /// <summary>
    ///     Makes a contribution a cell in this frame, tracking which renderer made the change.
    /// </summary>
    /// <param name="renderer">The renderer making the contribution.</param>
    /// <param name="pos">The position of the cell.</param>
    /// <param name="color">The color to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="character">The character to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="characterColor">The character color to set, or <c>null</c> to leave unchanged.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the frame bounds.</exception>
    public void Contribute(Renderer renderer, VectorInt pos, Color? color = null, char? character = null,
        Color? characterColor = null)
    {
        ArgumentNullException.ThrowIfNull(renderer);
        if (pos.X < 0 || pos.X >= Size.X || pos.Y < 0 || pos.Y >= Size.Y)
        {
            throw new ArgumentOutOfRangeException(nameof(pos), pos,
                "Contribution must be within the bounds of the Frame");
        }

        ref var cell = ref Cells[pos.X, pos.Y];

        if (color is { } colorValue)
        {
            cell.Color = colorValue;
        }

        if (character is { } characterValue)
        {
            cell.Char = characterValue;
            cell.CharColor = default(BasicColor);
        }

        if (characterColor is { } characterColorValue)
        {
            cell.CharColor = characterColorValue;
        }
    }

    internal void Reset(Cell cell = default)
    {
        for (var x = 0; x < Size.X; x++)
        for (var y = 0; y < Size.Y; y++)
        {
            Cells[x, y] = cell;
        }
    }
}