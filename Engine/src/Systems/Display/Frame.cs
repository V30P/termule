namespace Termule.Systems.Display;

using Types;
using Components;
using System;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Content that <see cref="Renderer"/>s contribute to during the render process.
/// </summary>
public sealed class FrameBuffer : Content
{
    internal FrameBuffer(int width, int height)
    : base(width, height)
    {
    }

    /// <summary>
    /// Gets the <see cref="Cell"/> at the given position.
    /// </summary>
    /// <param name="x">The X position of the cell to get.</param>
    /// <param name="y">The Y position of the cell to get.</param>
    /// <returns>The <see cref="Cell"/> at the specified position.</returns>
    public Cell this[int x, int y] => this.Cells[x, y];

    /// <summary>
    /// Makes a contribution a cell in this frame, tracking which renderer made the change.
    /// </summary>
    /// <param name="renderer">The renderer making the contribution.</param>
    /// <param name="pos">The position of the cell.</param>
    /// <param name="color">The color to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="character">The character to set, or <c>null</c> to leave unchanged.</param>
    /// <param name="characterColor">The character color to set, or <c>null</c> to leave unchanged.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the frame bounds.</exception>
    public void Contribute(Renderer renderer, VectorInt pos, Color? color = null, char? character = null, Color? characterColor = null)
    {
        ArgumentNullException.ThrowIfNull(renderer);
        if (pos.X < 0 || pos.X >= this.Size.X || pos.Y < 0 || pos.Y >= this.Size.Y)
        {
            throw new ArgumentOutOfRangeException(nameof(pos), pos, "Contribution must be within the bounds of the Frame");
        }

        ref Cell cell = ref this.Cells[pos.X, pos.Y];

        if (color is Color colorValue)
        {
            cell.Color = colorValue;
        }

        if (character is char characterValue)
        {
            cell.Char = characterValue;
            cell.CharColor = default(BasicColor);
        }

        if (characterColor is Color characterColorValue)
        {
            cell.CharColor = characterColorValue;
        }
    }

    internal void Reset(Cell cell = default)
    {
        for (int x = 0; x < this.Size.X; x++)
        {
            for (int y = 0; y < this.Size.Y; y++)
            {
                this.Cells[x, y] = cell;
            }
        }
    }
}